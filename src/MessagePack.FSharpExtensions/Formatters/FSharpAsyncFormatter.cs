using MessagePack.Formatters;
using Microsoft.FSharp.Control;

namespace MessagePack.FSharp.Formatters
{
    public sealed class FSharpAsyncFormatter<T> : IMessagePackFormatter<FSharpAsync<T>>
    {

        public FSharpAsyncFormatter() { }

        public void Serialize(ref MessagePackWriter writer, FSharpAsync<T> value, MessagePackSerializerOptions options) {
            if (value == null) {
                writer.WriteNil();
            } else {
                var v = FSharpAsync.RunSynchronously(value, null, null);

                options.Resolver.GetFormatterWithVerify<FSharpAsync<T>>().Serialize(ref writer, value, options);
            }
        }

        public FSharpAsync<T> Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options) {
            if (!reader.TryReadNil()) {
                var v = options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options);
                return Microsoft.FSharp.Core.ExtraTopLevelOperators.DefaultAsyncBuilder.Return(v);
            }

            return null;
        }
    }
}
