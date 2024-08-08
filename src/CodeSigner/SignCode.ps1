function Get-Files {
    param (
        [string]$dir,
        [string[]]$fileExtensions
    )

    $filePaths = @()

    try {
        $fileExtensionsCount = $fileExtensions.Length

        Get-ChildItem -Path $dir | ForEach-Object {
            $fileExtension = $_.Extension

            if ($fileExtensionsCount -eq 0 -or $fileExtensions -contains $fileExtension) {
                $filePaths += $_.FullName
            }
        }

        Get-ChildItem -Path $dir -Directory | ForEach-Object {
            $filePaths += Get-Files -dir $_.FullName -fileExtensions $fileExtensions
        }
    }
    catch {
        Write-Host $_.Exception.Message
    }

    return $filePaths
}

function Get-Classes {
    param (
        [string[]]$filePaths
    )

    $classDeclarations = @{}

    foreach ($filePath in $filePaths) {
        $csFileText = Get-Content -Path $filePath -Raw
        $csFileTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($csFileText)
        $csFileRoot = $csFileTree.GetRoot()

        $classDeclaration = $csFileRoot.DescendantNodes().Where({ $_.IsKind([Microsoft.CodeAnalysis.CSharp.SyntaxKind]::ClassDeclaration) }, 'First')

        if ($classDeclaration) {
            $classDeclarations[$filePath] = $classDeclaration
        }
    }

    return $classDeclarations
}

function Get-ConstantInsertingString {
    param (
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$classDeclarationSyntax,
        [string]$key
    )

    $addingStrings = @()
    $modifiers = 'private const'
    $insertingIndex = -1

    $field = $classDeclarationSyntax.DescendantNodes().Where({ $_.IsKind([Microsoft.CodeAnalysis.CSharp.SyntaxKind]::FieldDeclaration) }, 'Last')

    if (-not $field) {
        $members = $classDeclarationSyntax.Members

        if ($members.Count -eq 0) {
            return $addingStrings
        }

        $insertingIndex = $members[0].FullSpan.Start
    }
    else {
        $insertingIndex = $field.FullSpan.End
    }

    $endLine = if (-not $field) { "`n`n" } else { "`n" }
    $fieldName = Generate-VariableName
    $insertingString = "$modifiers string $fieldName = `"$key`";$endLine"
    $addingStrings += New-Object PSObject -Property @{ InsertingIndex = $insertingIndex; InsertingString = $insertingString }

    return $addingStrings
}

function Get-MethodsInsertingString {
    param (
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$classDeclarationSyntax,
        [string]$key
    )

    $addingStrings = @()
    $methods = $classDeclarationSyntax.DescendantNodes().Where({ $_.IsKind([Microsoft.CodeAnalysis.CSharp.SyntaxKind]::MethodDeclaration) }).Select({ $_ -as [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] }).ToArray()

    if ($methods.Length -eq 0) {
        return $addingStrings
    }

    $random = New-Object System.Random
    $selectedMethodsCount = $random.Next(1, $methods.Length + 1)
    $methodsIndexes = @()

    for ($i = 0; $i -lt $selectedMethodsCount; $i++) {
        $methodIndex = $random.Next(0, $methods.Length)

        while ($methodsIndexes.Contains($methodIndex)) {
            $methodIndex = $random.Next(0, $methods.Length)
        }

        $methodsIndexes += $methodIndex
        $method = $methods[$methodIndex]

        if (-not $method.Body) {
            continue
        }

        $bodyParts = $method.Body.DescendantNodes().Where({ $_.IsKind([Microsoft.CodeAnalysis.CSharp.SyntaxKind]::ExpressionStatement) }).Select({ $_ -as [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax] }).ToArray()

        if ($bodyParts.Length -eq 0) {
            continue
        }

        $bodyIndex = $random.Next(0, $bodyParts.Length)
        $insertingIndex = $bodyParts[$bodyIndex].FullSpan.End
        $fieldName = Generate-VariableName

        # TODO: Добавить рандомный выбор кода для методов
        $insertingString = "string $fieldName = `"$key`"`n"
        $insertingString += "if ($fieldName -eq `$($fieldName)) {`n"
        $insertingString += "    $fieldName = `"$(Generate-VariableName)`"`n"
        $insertingString += "}`n`n"
        $insertingString += "var `$(Generate-VariableName) = `$($fieldName)`n"

        $addingStrings += New-Object PSObject -Property @{ InsertingIndex = $insertingIndex; InsertingString = $insertingString }
    }

    return $addingStrings
}

function Generate-VariableName {
    $Choices = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    $MinLength = 4
    $MaxLength = 15

    $length = Get-Random -Minimum $MinLength -Maximum $MaxLength
    $randomString = Generate-RandomString -Length $length -CharSet $Choices

    if ($randomString[0] -match '\d') {
        $randomString = "a$randomString"
    }

    return $randomString
}

function Generate-RandomString {
    param (
        [int]$Length,
        [string]$CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
    )

    $charArray = $CharSet.ToCharArray() | Select-Object -Unique
    $result = @()

    for ($i = 0; $i -lt $Length; $i++) {
        $randomIndex = Get-Random -Maximum $charArray.Length
        $result += $charArray[$randomIndex]
    }

    return -join $result
}

# dotnet add package Microsoft.CodeAnalysis.CSharp --version 4.10.0
Install-PackageProvider -Name NuGet
$temp = Get-PackageSource
Install-Package -Name Microsoft.CodeAnalysis.CSharp -Source $temp[0].Source -ProviderName NuGet -Scope CurrentUser -Destination .\nuget -Force
$crpath = Resolve-Path ".\nuget\Microsoft.Rest.ClientRuntime.2.3.22\lib\netstandard2.0\Microsoft.Rest.ClientRuntime.dll"
[System.Reflection.Assembly]::LoadFrom($crpath)

$projectPath = "E:\Code\Products_Synthesis\src\LNADesigner"
$filePaths = Get-Files -dir $projectPath -fileExtensions ".cs"
$classDeclarations = Get-Classes -filePaths $filePaths

foreach ($keyValuePair in $classDeclarations.GetEnumerator()) {
    $insertingStrings = Get-ConstantInsertingString -classDeclarationSyntax $keyValuePair.Value -key "Test"
    $insertingStrings += Get-MethodsInsertingString -classDeclarationSyntax $keyValuePair.Value -key "Test"

    $builder = [System.Text.StringBuilder]::new((Get-Content -Path $keyValuePair.Key -Raw))
    foreach ($tuple in $insertingStrings | Sort-Object -Property InsertingIndex -Descending) {
        $builder.Insert($tuple.InsertingIndex, $tuple.InsertingString)
    }

    Set-Content -Path $keyValuePair.Key -Value $builder.ToString() -Encoding UTF8
}

Write-Host