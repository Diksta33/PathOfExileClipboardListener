using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExileClipboardListener.JSON
{
    public class DataContracts
    {
        [DataContract]
        public class JSONCharacter
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "league")]
            public string League { get; set; }

            [DataMember(Name = "class")]
            public string Class { get; set; }

            [DataMember(Name = "classId")]
            public int ClassId { get; set; }

            [DataMember(Name = "level")]
            public int Level { get; set; }
        }

        [DataContract]
        public class JSONProperty
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "values")]
            public List<object> Values { get; set; }

            [DataMember(Name = "displayMode")]
            public int DisplayMode { get; set; }
        }

        [DataContract]
        public class JSONAdditionalProperty
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "values")]
            public List<List<object>> Values { get; set; }

            [DataMember(Name = "displayMode")]
            public int DisplayMode { get; set; }

            [DataMember(Name = "progress")]
            public double Progress { get; set; }
        }

        [DataContract]
        public class JSONRequirement
        {
            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "values")]
            public List<object> Value { get; set; }

            [DataMember(Name = "displayMode")]
            public int DisplayMode { get; set; }
        }

        [DataContract]
        public class JSONItem
        {
            [DataMember(Name = "verified")]
            public bool Verified { get; set; }

            [DataMember(Name = "w")]
            public int Width { get; set; }

            [DataMember(Name = "h")]
            public int Height { get; set; }

            [DataMember(Name = "icon")]
            public string Icon { get; set; }

            [DataMember(Name = "support")]
            public bool Support { get; set; }

            [DataMember(Name = "league")]
            public string League { get; set; }

            [DataMember(Name = "name")]
            public string Name { get; set; }

            [DataMember(Name = "typeLine")]
            public string TypeLine { get; set; }

            [DataMember(Name = "identified")]
            public bool Identified { get; set; }

            [DataMember(Name = "properties")]
            public List<JSONProperty> Properties { get; set; }

            [DataMember(Name = "explicitMods")]
            public List<string> ExplicitMods { get; set; }

            [DataMember(Name = "descrText")]
            public string DescrText { get; set; }

            [DataMember(Name = "frameType")]
            public int FrameType { get; set; }

            [DataMember(Name = "x")]
            public int X { get; set; }

            [DataMember(Name = "y")]
            public int Y { get; set; }

            [DataMember(Name = "inventoryId")]
            public string InventoryId { get; set; }

            [DataMember(Name = "socketedItems")]
            public List<JSONItem> SocketedItems { get; set; }

            [DataMember(Name = "sockets")]
            public List<JSONSocket> Sockets { get; set; }

            [DataMember(Name = "additionalProperties")]
            public List<JSONAdditionalProperty> AdditionalProperties { get; set; }

            [DataMember(Name = "secDescrText")]
            public string SecDescrText { get; set; }

            [DataMember(Name = "implicitMods")]
            public List<string> ImplicitMods { get; set; }

            [DataMember(Name = "flavourText")]
            public List<string> FlavourText { get; set; }

            [DataMember(Name = "requirements")]
            public List<JSONRequirement> Requirements { get; set; }

            [DataMember(Name = "nextLevelRequirements")]
            public List<JSONRequirement> NextLevelRequirements { get; set; }

            [DataMember(Name = "socket")]
            public int Socket { get; set; }

            [DataMember(Name = "colour")]
            public string Color { get; set; }
        }

        [DataContract]
        public class JSONSocket
        {
            [DataMember(Name = "attr")]
            public string Attribute { get; set; }

            [DataMember(Name = "group")]
            public int Group { get; set; }
        }

        [DataContract(Name = "RootObject")]
        public class JSONStash
        {
            [DataMember(Name = "numTabs")]
            public int NumTabs { get; set; }

            [DataMember(Name = "items")]
            public List<JSONItem> Items { get; set; }

            [DataMember(Name = "tabs")]
            public List<JSONTab> Tabs { get; set; }
        }

        [DataContract(Name = "RootObject")]
        public class JSONInventory
        {
            [DataMember(Name = "items")]
            public List<JSONItem> Items { get; set; }
        }

        public class Colour
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }
        }

        public class JSONTab
        {
            public string N { get; set; }
            public int I { get; set; }
            public Colour Colour { get; set; }
            public string SrcL { get; set; }
            public string SrcC { get; set; }
            public string SrcR { get; set; }
        }
    }
}
