using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var projectPath = "";
var filePaths = GetFiles(projectPath, ".cs");
var classDeclarations = GetClasses(filePaths);
foreach (var keyValuePair in classDeclarations)
{
    var insertingStrings = GetConstantInsertingString(keyValuePair.Value, "Test");
    insertingStrings.AddRange(GetMethodsInsertingString(keyValuePair.Value, "Test"));
    var builder = new StringBuilder(File.ReadAllText(keyValuePair.Key));
    foreach (var tuple in insertingStrings.OrderByDescending(str => str.InsertingIndex))
    {
        builder.Insert(tuple.InsertingIndex, tuple.InsertingString);
    }

    File.WriteAllText(keyValuePair.Key, builder.ToString(), Encoding.UTF8);
}

Console.WriteLine();

List<string> GetFiles(string dir, params string[] fileExtensions)
{
    var filePaths = new List<string>();
    try
    {
        var fileExtensionsCount = fileExtensions.Length;
        foreach (var f in Directory.GetFiles(dir))
        {
            var fileExtension = Path.GetExtension(f);
            if (fileExtensionsCount == 0 || fileExtensions.Contains(fileExtension))
            {
                filePaths.Add(f);
            }
        }
        foreach (var d in Directory.GetDirectories(dir))
        {
            filePaths.AddRange(GetFiles(d, fileExtensions));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    return filePaths;
}

Dictionary<string, ClassDeclarationSyntax> GetClasses(List<string> filePaths)
{
    var classDeclarations = new Dictionary<string, ClassDeclarationSyntax>();
    foreach (var filePath in filePaths)
    {
        var csFileText = File.ReadAllText(filePath);
        var csFileTree = CSharpSyntaxTree.ParseText(csFileText);
        var csFileRoot = csFileTree.GetRoot();
        if (csFileRoot
                .DescendantNodes()
                .FirstOrDefault(n => n.IsKind(SyntaxKind.ClassDeclaration)) is
            ClassDeclarationSyntax classDeclaration)
        {
            classDeclarations[filePath] = classDeclaration;
        }
    }

    return classDeclarations;
}

List<(int InsertingIndex, string InsertingString)> GetConstantInsertingString(
    ClassDeclarationSyntax classDeclarationSyntax, string key)
{
    var addingStrings = new List<(int InsertingIndex, string InsertingString)>();
    var field = classDeclarationSyntax
        .DescendantNodes()
        .Where(node => node.IsKind(SyntaxKind.FieldDeclaration))
        .Select(node => (FieldDeclarationSyntax) node)
        .LastOrDefault();
    var modifiers = "private const";
    var insertingIndex = -1;
    if (field == null)
    {
        var members = classDeclarationSyntax.Members;
        if (members.Count == 0)
        {
            return new List<(int InsertingIndex, string InsertingString)>(0);
        }

        insertingIndex = members.First().FullSpan.Start;
    }
    else
    {
        insertingIndex = field.FullSpan.End;
    }

    var endLine = field == null ? "\n\n" : "\n";

    // TODO: Добавить рандомный выбор переменной
    var fieldName = "TestConstant";

    var insertingString = $"{modifiers} string {fieldName} = \"{key}\";{endLine}";
    addingStrings.Add((insertingIndex, insertingString));
    return addingStrings;
}

List<(int InsertingIndex, string InsertingString)> GetMethodsInsertingString(
    ClassDeclarationSyntax classDeclarationSyntax, string key)
{
    var addingStrings = new List<(int InsertingIndex, string InsertingString)>();
    var methods = classDeclarationSyntax
        .DescendantNodes()
        .Where(node => node.IsKind(SyntaxKind.MethodDeclaration))
        .Select(node => (MethodDeclarationSyntax)node)
        .ToArray();
    if (methods.Length == 0)
    {
        return new List<(int InsertingIndex, string InsertingString)>(0);
    }

    var random = new Random();
    var selectedMethodsCount = random.Next(1, methods.Length + 1);
    var methodsIndexes = new List<int>();
    for (int i = 0; i < selectedMethodsCount; i++)
    {
        var methodIndex = random.Next(0, methods.Length);
        while (methodsIndexes.Contains(methodIndex))
        {
            methodIndex = random.Next(0, methods.Length);
        }

        methodsIndexes.Add(methodIndex);
        var method = methods[methodIndex];
        if (method.Body == null)
        {
            continue;
        }

        var bodyParts =
            method.Body.DescendantNodes()
                .Where(n => n.IsKind(SyntaxKind.ExpressionStatement))
                .Select(n => (ExpressionStatementSyntax)n)
                .ToArray();
        if (bodyParts.Length == 0)
        {
            continue;
        }

        var bodyIndex = random.Next(0, bodyParts.Length);
        var insertingIndex = bodyParts[bodyIndex].FullSpan.End;

        // TODO: Добавить рандомный выбор переменной
        var fieldName = "TestConstant";
        var insertingString = $"string {fieldName} = \"{key}\";\n";
        insertingString +=
            $"if ({fieldName} == nameof({fieldName}))\n"
            + "{\n"
            + $"{fieldName} = \"123\";\n"
            + "}\n\n"
            + $"var msaeqwe123 = {fieldName};\n";

        addingStrings.Add((insertingIndex, insertingString));
    }

    return addingStrings;
}