namespace Crm.Link.RabbitMq.Messages
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.210.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("AccountEvent", Namespace = "", AnonymousType = true)]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AccountEvent", Namespace = "")]
    public partial class AccountEvent
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
        [System.Xml.Serialization.XmlElementAttribute("Name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLengthAttribute(50)]
        [System.Xml.Serialization.XmlElementAttribute("LastName", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LastName { get; set; }

        [System.ComponentModel.DataAnnotations.RegularExpressionAttribute("[^@]+@[^\\.]+\\..+")]
        [System.Xml.Serialization.XmlElementAttribute("Email", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.MinLengthAttribute(12)]
        [System.Xml.Serialization.XmlElementAttribute("VatNumber", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string VatNumber { get; set; }
    }
}
