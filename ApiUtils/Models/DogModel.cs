using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Q.Models
{
    public class DogData
    {
        public Dog Data { get; set; }
        public Links Links { get; set; }
    }

    public class Dog
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public Attributes Attributes { get; set; }
        public Relationships Relationships { get; set; }
    }

    public class Attributes
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public LifeSpan Life { get; set; }
        public Weight Male_Weight { get; set; }
        public Weight Female_Weight { get; set; }
        public bool Hypoallergenic { get; set; }
    }

    public class LifeSpan
    {
        public int Max { get; set; }
        public int Min { get; set; }
    }

    public class Weight
    {
        public int Max { get; set; }
        public int Min { get; set; }
    }

    public class Relationships
    {
        public Group Group { get; set; }
    }

    public class Group
    {
        public GroupData Data { get; set; }
    }

    public class GroupData
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }

    public class Links
    {
        public string Self { get; set; }
    }
}