using System;
using System.Collections.Generic;
using System.Text;
using Acidmanic.Utilities.Results;

namespace Acidmanic.Utilities.NamingConventions
{
    public class NamingConvention
    {
        public Result<ProcessedName> Parse(string name)
            {
                foreach (var convention in ConventionDescriptor.Standard.StandardConventions)
                {
                    if (Confirms(name, convention))
                    {
                        return new Result<ProcessedName>(true, new ProcessedName
                        {
                            Convention = convention,
                            Segments = Separate(name, convention)
                        });
                    }
                }

                return new Result<ProcessedName>().FailAndDefaultValue();
            }

            public bool Confirms(string name, ConventionDescriptor convention)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(convention.PreFix) && !name.StartsWith(convention.PreFix))
                {
                    return false;
                }

                if (ContainsUnAllowedChars(name, convention.PreFix, convention.Delimiter))
                {
                    return false;
                }

                var segments = Separate(name, convention);

                for (int i = 0; i < segments.Length; i++)
                {
                    var segment = segments[i];

                    var @case = convention.SegmentCase(i);

                    if (!Confirms(segment, @case))
                    {
                        return false;
                    }
                }

                return true;
            }

            private bool ContainsUnAllowedChars(string name, string conventionPreFix, string conventionDelimiter)
            {
                var pureName = name;

                if (!string.IsNullOrEmpty(conventionPreFix))
                {
                    pureName = pureName.Substring(conventionPreFix.Length, name.Length - conventionPreFix.Length);
                }

                if (!string.IsNullOrEmpty(conventionDelimiter))
                {
                    pureName = pureName.Replace(conventionDelimiter, "");
                }

                if (string.IsNullOrWhiteSpace(pureName))
                {
                    return true;
                }

                var chars = pureName.ToCharArray();

                if (char.IsDigit(chars[0]))
                {
                    return true;
                }

                for (int i = 0; i < chars.Length; i++)
                {
                    if (!char.IsLetterOrDigit(chars[i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            private string[] Separate(string name, ConventionDescriptor convention)
            {
                if (!string.IsNullOrEmpty(convention.PreFix))
                {
                    name = name.Substring(convention.PreFix.Length, name.Length - convention.PreFix.Length);
                }

                if (convention.Separation == Separation.ByDelimiter)
                {
                    return name.Split(convention.Delimiter, StringSplitOptions.None);
                }

                bool hitCaseUpper = convention.Separation != Separation.ByLowerCaseHit;

                var chars = name.ToCharArray();

                var segments = new List<string>();

                var current = "";

                var firstChar = true;

                foreach (var c in chars)
                {
                    // If hit
                    if (char.IsUpper(c) == hitCaseUpper)
                    {
                        // avoid adding empty entry while hiting segment-start at first character
                        if (!firstChar || !string.IsNullOrEmpty(current))
                        {
                            segments.Add(current);
                        }

                        current = "";
                    }

                    current += c;

                    firstChar = false;
                }

                if (!string.IsNullOrEmpty(current))
                {
                    segments.Add(current);
                }

                return segments.ToArray();
            }

            private bool Confirms(string segment, Case @case)
            {
                if (string.IsNullOrWhiteSpace(segment))
                {
                    return true;
                }

                var chars = segment.ToCharArray();

                var firstUpper = char.IsUpper(chars[0]);
                var restUpper = true;
                var restLower = true;

                for (int i = 1; i < chars.Length; i++)
                {
                    if (char.IsUpper(chars[i]))
                    {
                        restLower = false;
                    }

                    if (char.IsLower(chars[i]))
                    {
                        restUpper = false;
                    }
                }

                if (@case == Case.Lower)
                {
                    return (!firstUpper) && restLower;
                }

                if (@case == Case.Upper)
                {
                    return firstUpper && restUpper;
                }

                if (@case == Case.Capital)
                {
                    return firstUpper && restLower;
                }
                // Inverse Capital

                return !firstUpper && restUpper;
            }

            public string Render(string[] segments, ConventionDescriptor convention)
            {
                return Render(new ProcessedName
                {
                    Convention = convention,
                    Segments = segments
                });
            }

            public string Render(ProcessedName processedName)
            {
                var sb = new StringBuilder();

                sb.Append(processedName.Convention.PreFix);

                var segments = new string[processedName.Segments.Length];

                for (int i = 0; i < segments.Length; i++)
                {
                    segments[i] = SetCase(processedName.Segments[i], processedName.Convention.SegmentCase(i));
                }

                sb.Append(string.Join(processedName.Convention.Delimiter, segments));

                return sb.ToString();
            }

            private string SetCase(string value, Case conventionSegmentCase)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (conventionSegmentCase == Case.Upper)
                    {
                        return value?.ToUpper();
                    }

                    if (conventionSegmentCase == Case.Lower)
                    {
                        return value?.ToLower();
                    }

                    var first = value.Substring(0, 1);
                    var rest = value.Substring(1, value.Length - 1);

                    if (conventionSegmentCase == Case.Capital)
                    {
                        return first.ToUpper() + rest.ToLower();
                    }
                    // Inverse Capital

                    return first.ToLower() + rest.ToUpper();
                }

                return "";
            }


            public string Convert(string name, ConventionDescriptor convention)
            {
                var parsed = Parse(name);
                
                if (parsed)
                {
                    var converted = Render(parsed.Value.Segments, convention);
                }

                return name;
            }
    }
}