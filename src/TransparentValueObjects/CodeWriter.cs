using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;

namespace TransparentValueObjects;

public sealed class CodeWriter
{
    private const char NewLine = '\n';

    private readonly StringBuilder _stringBuilder = new();
    private int _depth;

    public CodeBlock AddBlock() => new(this);
    public RegionBlock AddRegionBlock(string regionName) => new(this, regionName);

    private void Indent()
    {
        if (_stringBuilder.Length == 0) return;
        if (_stringBuilder[_stringBuilder.Length - 1] != NewLine) return;
        _stringBuilder.Append(new string('\t', _depth));
    }

    public CodeWriter AppendUnindented([LanguageInjection("csharp")] string text)
    {
        _stringBuilder.Append(text);
        return this;
    }

    public CodeWriter AppendLineUnindented([LanguageInjection("csharp")] string line)
    {
        return AppendUnindented(line).AppendLine();
    }

    public CodeWriter Append([LanguageInjection("csharp")] string text)
    {
        Indent();
        _stringBuilder.Append(text);
        return this;
    }

    public CodeWriter AppendLine([LanguageInjection("csharp")] string line)
    {
        return Append(line).AppendLine();
    }

    public CodeWriter AppendLine()
    {
        if (_stringBuilder.Length < 2)
        {
            _stringBuilder.Append(NewLine);
            return this;
        }

        if (_stringBuilder[_stringBuilder.Length - 1] == NewLine &&
            _stringBuilder[_stringBuilder.Length - 2] == NewLine)
        {
            return this;
        }

        _stringBuilder.Append(NewLine);
        return this;
    }

    public override string ToString() => _stringBuilder.ToString();

    public void Dispose()
    {
        _depth--;
        AppendLine("}");
        AppendLine();
    }

    public readonly struct CodeBlock : IDisposable
    {
        private readonly CodeWriter _cw;
        public CodeBlock(CodeWriter cw)
        {
            _cw = cw;
            _cw.AppendLine("{");
            _cw._depth++;
        }

        public void Dispose()
        {
            _cw._depth--;
            _cw.AppendLine("}");
            _cw.AppendLine();
        }
    }

    public readonly struct RegionBlock : IDisposable
    {
        private readonly CodeWriter _cw;
        private readonly string _regionName;

        public RegionBlock(CodeWriter cw, string regionName)
        {
            _cw = cw;
            _regionName = regionName;

            _cw.AppendLine();
            _cw.AppendLineUnindented($"#region {regionName}");
            _cw.AppendLine();
        }

        public void Dispose()
        {
            _cw.AppendLine();
            _cw.AppendLineUnindented($"#endregion {_regionName}");
            _cw.AppendLine();
        }
    }
}
