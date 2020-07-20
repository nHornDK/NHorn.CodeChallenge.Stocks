using NHorn.CodeChallenge.Stocks.Models.Dto;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHorn.CodeChallenge.Stocks.Infrastructure
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            builder.Global(g => g
                .RootNamespace("NHorn.CodeChallenge.Stocks")
                .DontWriteWarningComment()
                .CamelCaseForProperties()
                .AutoOptionalProperties()
                .UseModules(true, true)
            );
            ConfigureModel<StockDto>(builder);
        }
        public static InterfaceExportBuilder<T> ConfigureModel<T>(ConfigurationBuilder builder)
        {
            return builder.ExportAsInterface<T>()
                .WithAllProperties()
                .AutoI(false)
                .Substitute(typeof(System.DateTime), new RtSimpleTypeName("Date"))
                .DontIncludeToNamespace();

        }
    }
}
