using System.Reflection;

namespace Core;

/// <summary>
/// Класс с инструментами для работы с рефлексией.
/// </summary>
public static class ReflectionTools
{
    /// <summary>
    /// Возвращает все реализации интерфейса из указанной сборки. Ищутся только те реализации,
    /// которые являются публичными не абстрактными классами.
    /// </summary>
    /// <param name="interfaceType">Тип интерфейса.</param>
    /// <param name="assembly">Сборка, в которой будет проводиться поиск реализаций.</param>
    /// <returns>Коллекция типов, реализующих указанный интерфейс.</returns>
    /// <exception cref="ArgumentException">Возникает, когда метод вызывается для типа, не являющегося интерфейсом.</exception>
    public static IEnumerable<Type> GetInterfaceImplementations(this Type interfaceType, Assembly assembly)
    {
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException(
                $"Попытка получить реализации интерфейса для типа, не являющегося интерфейсом ({interfaceType.Name})",
                nameof(interfaceType));
        }

        var implementingTypes = new List<Type>();
        var publicTypes = assembly.GetTypes()
            .Where(type => type.IsPublic && !type.IsAbstract && type.IsClass);
        var blockTemplateImplementations = publicTypes.Where(
            type => type.GetInterface(interfaceType.Name) != null);
        implementingTypes.AddRange(blockTemplateImplementations);
        return implementingTypes;
    }
}