using System;

using Best.MQTT.Packets.Utils;

namespace Best.MQTT.Packets.Builders
{
    /// <summary>
    /// https://docs.oasis-open.org/mqtt/mqtt/v5.0/os/mqtt-v5.0-os.html#_Toc3901141
    /// </summary>
    internal struct PublishReleasePacketBuilder
    {
        MQTTClient _client;
        UInt16 _packetId;
        PublishReleaseAndCompleteReasonCodes _reasonCode;

        Properties _properties;

        public PublishReleasePacketBuilder(MQTTClient client)
        {
            this._client = client;
            this._packetId = 0;
            this._reasonCode = PublishReleaseAndCompleteReasonCodes.Success;
            this._properties = default(Properties);
        }

        public PublishReleasePacketBuilder WithPacketID(UInt16 pid)
        {
            this._packetId = pid;

            return this;
        }

        public PublishReleasePacketBuilder WithReasonCode(PublishReleaseAndCompleteReasonCodes code)
        {
            this._reasonCode = code;

            return this;
        }

        public PublishReleasePacketBuilder WithReasonString(string reason)
        {
            this._properties.AddProperty(new Property { Type = PacketProperties.ReasonString, Data = Data.FromString(reason) });

            return this;
        }

        public PublishReleasePacketBuilder WithUserProperty(string key, string value)
        {
            this._properties.AddProperty(new Property { Type = PacketProperties.UserProperty, Data = Data.FromStringPair(key, value) });

            return this;
        }

        public Packet Build()
        {
            var packet = new Packet();
            packet.Type = PacketTypes.PublishRelease;
            packet.Flags = new BitField(0b_0010);

            packet.AddVariableHeader(Data.FromTwoByteInteger(this._packetId));

            if (this._client.Options.ProtocolVersion >= SupportedProtocolVersions.MQTT_5_0)
            {
                packet.AddVariableHeader(Data.FromByte((byte)this._reasonCode));
                packet.AddVariableHeader(Data.FromProperties(this._properties));
            }

            return packet;
        }
    }
}
