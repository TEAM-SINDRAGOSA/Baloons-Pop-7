namespace BalloonsPop
{
    using System;

    [AttributeUsage(AttributeTargets.Struct |
    AttributeTargets.Class | AttributeTargets.Interface,
    AllowMultiple = true)]

    public class AuthorAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public AuthorAttribute(string name)
        {
            this.Name = name;
        }
    }
}
