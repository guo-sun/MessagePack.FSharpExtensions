// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Resolvers
{
    using System;
    using System.Buffers;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(13)
            {
                { typeof(global::MessagePack.Tests.DUTests.SimpleUnion), 0 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion), 1 },
                { typeof(global::MessagePack.Tests.DUTests.CallbackUnion), 2 },
                { typeof(global::MessagePack.Tests.RecordTest.SimpleRecord), 3 },
                { typeof(global::MessagePack.Tests.RecordTest.SimpleStringKeyRecord), 4 },
                { typeof(global::MessagePack.Tests.RecordTest.StructRecord), 5 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsE), 6 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsError), 7 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsF), 8 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsG), 9 },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsOk), 10 },
                { typeof(global::MessagePack.Tests.StructDUTest.StructCallbackUnion), 11 },
                { typeof(global::MessagePack.Tests.StructDUTest.StructUnion), 12 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new MessagePack.Formatters.MessagePack.Tests.SimpleUnionFormatter();
                case 1: return new MessagePack.Formatters.MessagePack.Tests.CsStructUnionFormatter();
                case 2: return new MessagePack.Formatters.MessagePack.Tests.DUTests_CallbackUnionFormatter();
                case 3: return new MessagePack.Formatters.MessagePack.Tests.RecordTest_SimpleRecordFormatter();
                case 4: return new MessagePack.Formatters.MessagePack.Tests.RecordTest_SimpleStringKeyRecordFormatter();
                case 5: return new MessagePack.Formatters.MessagePack.Tests.RecordTest_StructRecordFormatter();
                case 6: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_Compatibility_CsEFormatter();
                case 7: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_Compatibility_CsErrorFormatter();
                case 8: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_Compatibility_CsFFormatter();
                case 9: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_Compatibility_CsGFormatter();
                case 10: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_Compatibility_CsOkFormatter();
                case 11: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_StructCallbackUnionFormatter();
                case 12: return new MessagePack.Formatters.MessagePack.Tests.StructDUTest_StructUnionFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1649 // File name should match first type name



// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.MessagePack.Tests
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using MessagePack;

    public sealed class SimpleUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.DUTests.SimpleUnion>
    {
        private readonly Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
        private readonly Dictionary<int, int> keyToJumpMap;

        public SimpleUnionFormatter()
        {
            this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(3, global::MessagePack.Internal.RuntimeTypeHandleEqualityComparer.Default)
            {
                { typeof(global::MessagePack.Tests.DUTests.SimpleUnion._A).TypeHandle, new KeyValuePair<int, int>(0, 0) },
                { typeof(global::MessagePack.Tests.DUTests.SimpleUnion.B).TypeHandle, new KeyValuePair<int, int>(1, 1) },
                { typeof(global::MessagePack.Tests.DUTests.SimpleUnion.C).TypeHandle, new KeyValuePair<int, int>(2, 2) },
            };
            this.keyToJumpMap = new Dictionary<int, int>(3)
            {
                { 0, 0 },
                { 1, 1 },
                { 2, 2 },
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.DUTests.SimpleUnion value, global::MessagePack.MessagePackSerializerOptions options)
        {
            KeyValuePair<int, int> keyValuePair;
            if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
            {
                writer.WriteArrayHeader(2);
                writer.WriteInt32(keyValuePair.Key);
                switch (keyValuePair.Value)
                {
                    case 0:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion._A>().Serialize(ref writer, (global::MessagePack.Tests.DUTests.SimpleUnion._A)value, options);
                        break;
                    case 1:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion.B>().Serialize(ref writer, (global::MessagePack.Tests.DUTests.SimpleUnion.B)value, options);
                        break;
                    case 2:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion.C>().Serialize(ref writer, (global::MessagePack.Tests.DUTests.SimpleUnion.C)value, options);
                        break;
                    default:
                        break;
                }

                return;
            }

            writer.WriteNil();
        }

        public global::MessagePack.Tests.DUTests.SimpleUnion Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            if (reader.ReadArrayHeader() != 2)
            {
                throw new InvalidOperationException("Invalid Union data was detected. Type:global::MessagePack.Tests.DUTests.SimpleUnion");
            }

            options.Security.DepthStep(ref reader);
            var key = reader.ReadInt32();

            if (!this.keyToJumpMap.TryGetValue(key, out key))
            {
                key = -1;
            }

            global::MessagePack.Tests.DUTests.SimpleUnion result = null;
            switch (key)
            {
                case 0:
                    result = (global::MessagePack.Tests.DUTests.SimpleUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion._A>().Deserialize(ref reader, options);
                    break;
                case 1:
                    result = (global::MessagePack.Tests.DUTests.SimpleUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion.B>().Deserialize(ref reader, options);
                    break;
                case 2:
                    result = (global::MessagePack.Tests.DUTests.SimpleUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.DUTests.SimpleUnion.C>().Deserialize(ref reader, options);
                    break;
                default:
                    reader.Skip();
                    break;
            }

            reader.Depth--;
            return result;
        }
    }

    public sealed class CsStructUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion>
    {
        private readonly Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
        private readonly Dictionary<int, int> keyToJumpMap;

        public CsStructUnionFormatter()
        {
            this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(3, global::MessagePack.Internal.RuntimeTypeHandleEqualityComparer.Default)
            {
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsE).TypeHandle, new KeyValuePair<int, int>(0, 0) },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsF).TypeHandle, new KeyValuePair<int, int>(1, 1) },
                { typeof(global::MessagePack.Tests.StructDUTest.Compatibility.CsG).TypeHandle, new KeyValuePair<int, int>(2, 2) },
            };
            this.keyToJumpMap = new Dictionary<int, int>(3)
            {
                { 0, 0 },
                { 1, 1 },
                { 2, 2 },
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion value, global::MessagePack.MessagePackSerializerOptions options)
        {
            KeyValuePair<int, int> keyValuePair;
            if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
            {
                writer.WriteArrayHeader(2);
                writer.WriteInt32(keyValuePair.Key);
                switch (keyValuePair.Value)
                {
                    case 0:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsE>().Serialize(ref writer, (global::MessagePack.Tests.StructDUTest.Compatibility.CsE)value, options);
                        break;
                    case 1:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsF>().Serialize(ref writer, (global::MessagePack.Tests.StructDUTest.Compatibility.CsF)value, options);
                        break;
                    case 2:
                        options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsG>().Serialize(ref writer, (global::MessagePack.Tests.StructDUTest.Compatibility.CsG)value, options);
                        break;
                    default:
                        break;
                }

                return;
            }

            writer.WriteNil();
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            if (reader.ReadArrayHeader() != 2)
            {
                throw new InvalidOperationException("Invalid Union data was detected. Type:global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion");
            }

            options.Security.DepthStep(ref reader);
            var key = reader.ReadInt32();

            if (!this.keyToJumpMap.TryGetValue(key, out key))
            {
                key = -1;
            }

            global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion result = null;
            switch (key)
            {
                case 0:
                    result = (global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsE>().Deserialize(ref reader, options);
                    break;
                case 1:
                    result = (global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsF>().Deserialize(ref reader, options);
                    break;
                case 2:
                    result = (global::MessagePack.Tests.StructDUTest.Compatibility.CsStructUnion)options.Resolver.GetFormatterWithVerify<global::MessagePack.Tests.StructDUTest.Compatibility.CsG>().Deserialize(ref reader, options);
                    break;
                default:
                    reader.Skip();
                    break;
            }

            reader.Depth--;
            return result;
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name


// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.MessagePack.Tests
{
    using System;
    using System.Buffers;
    using MessagePack;

    public sealed class DUTests_CallbackUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.DUTests.CallbackUnion>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.DUTests.CallbackUnion value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            IFormatterResolver formatterResolver = options.Resolver;
            ((IMessagePackSerializationCallbackReceiver)value).OnBeforeSerialize();
            writer.WriteArrayHeader(1);
            writer.Write(value.Tag);
        }

        public global::MessagePack.Tests.DUTests.CallbackUnion Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Tag__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Tag__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.DUTests.CallbackUnion();
            ((IMessagePackSerializationCallbackReceiver)____result).OnAfterDeserialize();
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class RecordTest_SimpleRecordFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.RecordTest.SimpleRecord>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.RecordTest.SimpleRecord value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(3);
            writer.Write(value.Property1);
            writer.Write(value.Property2);
            writer.Write(value.Property3);
        }

        public global::MessagePack.Tests.RecordTest.SimpleRecord Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Property1__ = default(int);
            var __Property2__ = default(long);
            var __Property3__ = default(float);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Property1__ = reader.ReadInt32();
                        break;
                    case 1:
                        __Property2__ = reader.ReadInt64();
                        break;
                    case 2:
                        __Property3__ = reader.ReadSingle();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.RecordTest.SimpleRecord(__Property1__, __Property2__, __Property3__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class RecordTest_SimpleStringKeyRecordFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.RecordTest.SimpleStringKeyRecord>
    {


        private readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public RecordTest_SimpleStringKeyRecordFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Prop1", 0 },
                { "Prop2", 1 },
                { "Prop3", 2 },
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("Prop1"),
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("Prop2"),
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("Prop3"),
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.RecordTest.SimpleStringKeyRecord value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteMapHeader(3);
            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.Write(value.Prop1);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.Write(value.Prop2);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.Write(value.Prop3);
        }

        public global::MessagePack.Tests.RecordTest.SimpleStringKeyRecord Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __Prop1__ = default(int);
            var __Prop2__ = default(long);
            var __Prop3__ = default(float);

            for (int i = 0; i < length; i++)
            {
                ReadOnlySpan<byte> stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                int key;
                if (!this.____keyMapping.TryGetValue(stringKey, out key))
                {
                    reader.Skip();
                    continue;
                }

                switch (key)
                {
                    case 0:
                        __Prop1__ = reader.ReadInt32();
                        break;
                    case 1:
                        __Prop2__ = reader.ReadInt64();
                        break;
                    case 2:
                        __Prop3__ = reader.ReadSingle();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.RecordTest.SimpleStringKeyRecord(__Prop1__, __Prop2__, __Prop3__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class RecordTest_StructRecordFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.RecordTest.StructRecord>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.RecordTest.StructRecord value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            writer.Write(value.X);
            writer.Write(value.Y);
        }

        public global::MessagePack.Tests.RecordTest.StructRecord Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __X__ = default(int);
            var __Y__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __X__ = reader.ReadInt32();
                        break;
                    case 1:
                        __Y__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.RecordTest.StructRecord(__X__, __Y__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_Compatibility_CsEFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsE>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsE value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(0);
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsE Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.Compatibility.CsE();
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_Compatibility_CsErrorFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsError>
    {


        private readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public StructDUTest_Compatibility_CsErrorFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ErrorValue", 0 },
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("ErrorValue"),
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsError value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteMapHeader(1);
            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.ErrorValue, options);
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsError Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __ErrorValue__ = default(string);

            for (int i = 0; i < length; i++)
            {
                ReadOnlySpan<byte> stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                int key;
                if (!this.____keyMapping.TryGetValue(stringKey, out key))
                {
                    reader.Skip();
                    continue;
                }

                switch (key)
                {
                    case 0:
                        __ErrorValue__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.Compatibility.CsError(__ErrorValue__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_Compatibility_CsFFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsF>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsF value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(1);
            writer.Write(value.Item);
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsF Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Item__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Item__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.Compatibility.CsF(__Item__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_Compatibility_CsGFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsG>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsG value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            writer.Write(value.Item1);
            writer.Write(value.Item2);
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsG Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Item1__ = default(long);
            var __Item2__ = default(float);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Item1__ = reader.ReadInt64();
                        break;
                    case 1:
                        __Item2__ = reader.ReadSingle();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.Compatibility.CsG(__Item1__, __Item2__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_Compatibility_CsOkFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.Compatibility.CsOk>
    {


        private readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public StructDUTest_Compatibility_CsOkFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "ResultValue", 0 },
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("ResultValue"),
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.Compatibility.CsOk value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteMapHeader(1);
            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.Write(value.ResultValue);
        }

        public global::MessagePack.Tests.StructDUTest.Compatibility.CsOk Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __ResultValue__ = default(int);

            for (int i = 0; i < length; i++)
            {
                ReadOnlySpan<byte> stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                int key;
                if (!this.____keyMapping.TryGetValue(stringKey, out key))
                {
                    reader.Skip();
                    continue;
                }

                switch (key)
                {
                    case 0:
                        __ResultValue__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.Compatibility.CsOk(__ResultValue__);
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_StructCallbackUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.StructCallbackUnion>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.StructCallbackUnion value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            ((IMessagePackSerializationCallbackReceiver)value).OnBeforeSerialize();
            writer.WriteArrayHeader(1);
            writer.Write(value.Tag);
        }

        public global::MessagePack.Tests.StructDUTest.StructCallbackUnion Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Tag__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Tag__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.StructCallbackUnion();
            ((IMessagePackSerializationCallbackReceiver)____result).OnAfterDeserialize();
            reader.Depth--;
            return ____result;
        }
    }

    public sealed class StructDUTest_StructUnionFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::MessagePack.Tests.StructDUTest.StructUnion>
    {


        public void Serialize(ref MessagePackWriter writer, global::MessagePack.Tests.StructDUTest.StructUnion value, global::MessagePack.MessagePackSerializerOptions options)
        {
            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(1);
            writer.Write(value.Tag);
        }

        public global::MessagePack.Tests.StructDUTest.StructUnion Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var __Tag__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Tag__ = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::MessagePack.Tests.StructDUTest.StructUnion();
            reader.Depth--;
            return ____result;
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

