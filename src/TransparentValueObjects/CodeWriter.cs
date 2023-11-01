using System;
using System.Text;

namespace TransparentValueObjects;

public class CodeWriter : ICodeBlock
{
    private readonly StringBuilder _stringBuilder = new();
    private int _depth;

    public ICodeBlock AddBlock()
    {
        AppendLine("{");
        _depth++;
        return this;
    }

    public CodeWriter Append(string text)
    {
        _stringBuilder.Append(new string('\t', _depth));
        _stringBuilder.Append(text);
        return this;
    }

    public CodeWriter AppendLine(string line)
    {
        return Append(line).AppendLine();
    }

    public CodeWriter AppendLine()
    {
        _stringBuilder.AppendLine();
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
