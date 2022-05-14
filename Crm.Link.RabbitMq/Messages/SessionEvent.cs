using Crm.Link.RabbitMq.Messages;

namespace Crm.Link.RabbitMq.Messages
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("SessionEvent", Namespace = "", AnonymousType = true)]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SessionEvent", Namespace = "")]
    public partial class SessionEvent
    {

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(32)]
        [System.Xml.Serialization.XmlElementAttribute("UUID_nr", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UUID_Nr { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("SourceEntityId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal SourceEntityId { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(30)]
        [System.Xml.Serialization.XmlElementAttribute("EntityType", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string EntityType { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("EntityVersion", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal EntityVersion { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("Source", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SourceEnum Source { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("Method", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MethodEnum Method { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("Version", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal Version { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(30)]
        [System.Xml.Serialization.XmlElementAttribute("Title", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("StartDateUTC", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "dateTime")]
        public System.DateTime StartDateUTC { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("EndDateUTC", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "dateTime")]
        public System.DateTime EndDateUTC { get; set; }

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(32)]
        [System.Xml.Serialization.XmlElementAttribute("OrganiserUUID", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OrganiserUUID { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("IsActive", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool IsActive { get; set; }
    }
}
