namespace EasyAdmin.Shared.Common
{
    public class Template
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator Template(Ovirt.Template ovirtTemplate)
        {
            Template template = new Template
            {
                Name = ovirtTemplate.Name,
                Description = string.IsNullOrEmpty(ovirtTemplate.Description) ? ovirtTemplate.Name : ovirtTemplate.Description 
            };
            return template;
        }
    }
}
