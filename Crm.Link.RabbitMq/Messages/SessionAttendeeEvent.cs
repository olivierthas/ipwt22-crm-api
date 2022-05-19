namespace Crm.Link.RabbitMq.Messages
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("SessionAttendeeEvent", Namespace = "", AnonymousType = true)]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SessionAttendeeEvent", Namespace = "")]
    public partial class SessionAttendeeEvent
    {

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(32)]
        [System.Xml.Serialization.XmlElementAttribute("UUID_nr", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string? UUID_Nr { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("SourceEntityId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal SourceEntityId { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(30)]
        [System.Xml.Serialization.XmlElementAttribute("EntityType", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string? EntityType { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("EntityVersion", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal EntityVersion { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("Source", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SourceEnum Source { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("Method", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MethodEnum Method { get; set; }

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(32)]
        [System.Xml.Serialization.XmlElementAttribute("AttendeeUUID", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string? AttendeeUUID { get; set; }

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(32)]
        [System.Xml.Serialization.XmlElementAttribute("SessionUUID", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string? SessionUUID { get; set; }

        [System.Xml.Serialization.XmlElementAttribute("InvitationStatus", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InvitationStatusEnum InvitationStatus { get; set; }
    }
}
