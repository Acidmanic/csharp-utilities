using System;

namespace Acidmanic.Utilities.NamingConventions
{
    public class ConventionDescriptor
    {
        public static class Standard
            {
                public static ConventionDescriptor Pascal { get; } = new ConventionDescriptor
                {
                    Name = "Pascal",
                    Delimiter = "",
                    Separation = Separation.ByUpperCaseHit,
                    PreFix = "",
                    SegmentCase = i => Case.Capital
                };

                public static ConventionDescriptor Camel { get; } = new ConventionDescriptor
                {
                    Name = "Camel",
                    Delimiter = "",
                    Separation = Separation.ByUpperCaseHit,
                    PreFix = "",
                    SegmentCase = i => (i == 0) ? Case.Lower : Case.Capital
                };

                public static ConventionDescriptor UnderScoredCamel { get; } = new ConventionDescriptor
                {
                    Name = "UnderScoredCamel",
                    Delimiter = "",
                    Separation = Separation.ByUpperCaseHit,
                    PreFix = "_",
                    SegmentCase = i => (i == 0) ? Case.Lower : Case.Capital
                };

                public static ConventionDescriptor Kebab { get; } = new ConventionDescriptor
                {
                    Name = "Kebab",
                    Delimiter = "-",
                    Separation = Separation.ByDelimiter,
                    PreFix = "",
                    SegmentCase = i => Case.Lower
                };

                public static ConventionDescriptor Snake { get; } = new ConventionDescriptor
                {
                    Name = "Snake",
                    Delimiter = "_",
                    Separation = Separation.ByDelimiter,
                    PreFix = "",
                    SegmentCase = i => Case.Lower
                };

                public static ConventionDescriptor FatSnake { get; } = new ConventionDescriptor
                {
                    Name = "FatSnake",
                    Delimiter = "_",
                    Separation = Separation.ByDelimiter,
                    PreFix = "",
                    SegmentCase = i => Case.Upper
                };


                public static ConventionDescriptor[] StandardConventions =
                {
                    Pascal,
                    Camel,
                    UnderScoredCamel,
                    Kebab,
                    Snake,
                    FatSnake
                };
            }

            public string Name { get; set; }

            public Separation Separation { get; set; }

            public string Delimiter { get; set; }

            public Func<int, Case> SegmentCase { get; set; }

            public string PreFix { get; set; }
    }
}