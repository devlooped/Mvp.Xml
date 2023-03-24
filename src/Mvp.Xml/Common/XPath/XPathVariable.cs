namespace Mvp.Xml.XPath;

/// <summary>
/// Represents a variable to use in dynamic XPath expression 
/// queries.
/// </summary>
/// <remarks>Author: Daniel Cazzulino, <a href="https://cazzulino.com">blog</a></remarks>
public struct XPathVariable
{
    /// <summary>
    /// Initializes the new variable.
    /// </summary>
    /// <param name="name">The name to assign to the variable.</param>
    /// <param name="value">The variable value.</param>
    public XPathVariable(string name, object value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets the variable name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the variable value.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Checks equality of two variables. They are equal 
    /// if both their <see cref="Name"/> and their <see cref="Value"/> 
    /// are equal.
    /// </summary>
    public override bool Equals(object obj)
        => Name == ((XPathVariable)obj).Name && Value == ((XPathVariable)obj).Value;

    /// <summary>
    /// See <see cref="object.GetHashCode"/>.
    /// </summary>
    public override int GetHashCode() => (Name + "." + Value.GetHashCode()).GetHashCode();

    /// <summary>
    /// Checks equality of two variables. They are equal 
    /// if both their <see cref="Name"/> and their <see cref="Value"/> 
    /// are equal.
    /// </summary>
    public static bool operator ==(XPathVariable a, XPathVariable b) => a.Equals(b);

    /// <summary>
    /// Checks equality of two variables. They are not equal 
    /// if both their <see cref="Name"/> and their <see cref="Value"/> 
    /// are different.
    /// </summary>
    public static bool operator !=(XPathVariable a, XPathVariable b) => !a.Equals(b);
}
