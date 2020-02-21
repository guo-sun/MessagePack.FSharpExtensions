using MessagePack.Formatters;
using Microsoft.FSharp.Core;

namespace MessagePack.FSharp.Formatters
{
    public sealed class FSharpOptionFormatter<T> : IMessagePackFormatter<FSharpOption<T>>
    {
        public void Serialize(ref MessagePackWriter writer, FSharpOption<T> value, MessagePackSerializerOptions options)
        {
            if (FSharpOption<T>.get_IsNone(value))
            {
                writer.WriteNil();
            }
            else
            {
                options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value.Value, options);
            }
        }

        public FSharpOption<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (!reader.TryReadNil())
            {
                return FSharpOption<T>.Some(
                    options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options)
                );
            }

            return null;
        }
    }

    public sealed class StaticFSharpOptionFormatter<T> : IMessagePackFormatter<FSharpOption<T>>
    {
        readonly IMessagePackFormatter<T> underlyingFormatter;

        public StaticFSharpOptionFormatter(IMessagePackFormatter<T> underlyingFormatter)
        {
            this.underlyingFormatter = underlyingFormatter;
        }

        public void Serialize(ref MessagePackWriter writer, FSharpOption<T> value, MessagePackSerializerOptions options)
        {
            if (FSharpOption<T>.get_IsNone(value))
            {
                writer.WriteNil();
            }
            else
            {
                underlyingFormatter.Serialize(ref writer, value.Value, options);
            }
        }

        public FSharpOption<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (!reader.TryReadNil())
            {
                return FSharpOption<T>.Some(underlyingFormatter.Deserialize(ref reader, options));
            }

            return null;
        }
    }
}
