using System.Runtime.InteropServices;

public struct MarshalledString : IDisposable
{
    public string Value { get; }
    public nint Buffer { get; }

    public MarshalledString(string value)
    {
        Value = value;
        Buffer = Marshal.StringToHGlobalUni(value);
    }

    public void Dispose()
    {
        Marshal.FreeHGlobal(Buffer);
    }

    public static implicit operator nint(MarshalledString instance) => instance.Buffer;
    public static implicit operator string(MarshalledString instance) => instance.Value;
}