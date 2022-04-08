namespace Manage.Repository.Entities
{
    public class PropertyFeature
    {
        public string PropertyId { get; set; }
        public int FeatureId { get; set; }

        public Property Property { get; set; }
        public Feature Feature { get; set; }
    }
}
