using System;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.Exslt;

/// <summary>
/// This class implements the EXSLT functions in the http://exslt.org/random namespace.
/// </summary>
public class ExsltRandom
{
    /// <summary>
    ///  Implements the following function 
    ///     number+ random:random-sequence(number?, number?)
    /// </summary>				
    public XPathNodeIterator randomSequence() => randomSequenceImpl(1, (int)DateTime.Now.Ticks);

    /// <summary>
    /// This wrapper method will be renamed during custom build 
    /// to provide conformant EXSLT function name.
    /// </summary>	
    public XPathNodeIterator randomSequence_RENAME_ME() => randomSequence();

    /// <summary>
    ///  Implements the following function 
    ///     number+ random:random-sequence(number?, number?)
    /// </summary>	
    public XPathNodeIterator randomSequence(double number) => randomSequenceImpl(number, (int)DateTime.Now.Ticks);

    /// <summary>
    /// This wrapper method will be renamed during custom build 
    /// to provide conformant EXSLT function name.
    /// </summary>	
    public XPathNodeIterator randomSequence_RENAME_ME(double number) => randomSequence(number);

    /// <summary>
    ///  Implements the following function 
    ///     number+ random:random-sequence(number?, number?)
    /// </summary>
    public XPathNodeIterator randomSequence(double number, double seed) => randomSequenceImpl(number, (int)(seed % int.MaxValue));

    /// <summary>
    /// This wrapper method will be renamed during custom build 
    /// to provide conformant EXSLT function name.
    /// </summary>	
    public XPathNodeIterator randomSequence_RENAME_ME(double number, double seed) => randomSequence(number, seed);

    /// <summary>
    /// random-sequence() implementation;
    /// </summary>
    /// <param name="number"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    XPathNodeIterator randomSequenceImpl(double number, int seed)
    {
        var doc = new XmlDocument();
        doc.LoadXml("<randoms/>");

        if (seed == int.MinValue)
            seed += 1;

        var rand = new Random(seed);

        //Negative number is bad idea - fallback to default
        if (number < 0)
            number = 1;

        //we limit number of generated numbers to int.MaxValue
        if (number > int.MaxValue)
            number = int.MaxValue;

        for (var i = 0; i < Convert.ToInt32(number); i++)
        {
            var elem = doc.CreateElement("random");
            elem.InnerText = rand.NextDouble().ToString();
            doc.DocumentElement.AppendChild(elem);
        }

        return doc.CreateNavigator().Select("/randoms/random");
    }
}
