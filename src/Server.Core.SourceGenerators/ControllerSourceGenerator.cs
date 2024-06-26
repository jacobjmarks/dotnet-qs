﻿using Microsoft.CodeAnalysis;

namespace Server.Core.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public class ControllerSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        static string indent(int depth = 1) => new(' ', 4 * depth);

        static string randomMethodName() => "Method_" + Guid.NewGuid().ToString()[..6];

        var actions = Shared.Constants.Endpoints.Select(e =>
        $$"""
        [HttpGet, Route("{{e.Route}}")]
            public IActionResult {{randomMethodName()}}([FromQuery] {{e.Type}} q) => Json(q);
        """);

        context.AddSource($"Generated.Controller.g.cs",
        $$"""
        // <auto-generated/>
        #nullable enable
        using Microsoft.AspNetCore.Mvc;

        namespace {{context.Compilation.AssemblyName}};

        [ApiController]
        public class Controller : Microsoft.AspNetCore.Mvc.Controller
        {
            {{string.Join("\n\n" + indent(1), actions)}}
        }
        """);
    }
}
