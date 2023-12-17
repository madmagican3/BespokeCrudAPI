namespace BasicCrudAPI.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TTLAttribute : Attribute
    {
        public int TTLInMinutes { get; set; }

        public TTLAttribute(int TTLInMinutes)
        {
            this.TTLInMinutes = TTLInMinutes;
        }
    }
}
