using System;
using System.Text;

namespace TransparentValueObjects;

public class CodeWriter : ICodeBlock
{
    private const string NewLineString = "\n";

    private readonly StringBuilder _stringBuilder = new();
    private int _depth;

    public ICodeBlock AddBlock()
    {
        AppendLine("{");
        _depth++;
        return this;
    }

    private void Indent()
    {
        if (_stringBuilder.Length < NewLineString.Length) return;
        for (var i = 0; i < NewLineString.Length; i++)
        {
            var c = _stringBuilder[_stringBuilder.Length - NewLineString.Length - i];
            if (c != NewLineString[i]) return;
        }

        _stringBuilder.Append(new string('\t', _depth));
    }

    public CodeWriter Append(string text)
    {
        Indent();
        _stringBuilder.Append(text);
        return this;
    }

    public CodeWriter AppendLine(string line)
    {
        return Append(line).AppendLine();
    }

    public CodeWriter AppendLine()
    {
        _stringBuilder.Append(NewLineString);
        return this;
    }

    public override string ToString() => _stringBuilder.ToString();

    public void Dispose()
    {
        _depth--;
        AppendLine("}");
        AppendLine();
    }
}

public interface ICodeBlock : IDisposable { }
