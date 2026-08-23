using System;
using System.Linq;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components;

class Program
{
    static void Main()
    {
        var asm = typeof(FluentSelect<,>).Assembly;
        Console.WriteLine($"Assembly: {asm.FullName}");
        
        var methods = asm.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
            .Where(m => m.GetParameters().Length > 0 && typeof(Enum).IsAssignableFrom(m.GetParameters()[0].ParameterType))
            .ToList();

        foreach (var m in methods)
        {
            Console.WriteLine($"Found Enum extension: {m.DeclaringType?.FullName}.{m.Name}");
        }

        var allTypes = asm.GetTypes().Where(t => t.Name.Contains("Enum", StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var t in allTypes)
        {
            Console.WriteLine($"Enum related type: {t.FullName}");
        }
    }
}
