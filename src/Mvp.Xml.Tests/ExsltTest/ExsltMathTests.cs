using System;
using Xunit;

namespace ExsltTest;

/// <summary>
/// Collection of unit tests for EXSLT Math module functions.
/// </summary>
public class ExsltMathTests : ExsltUnitTests
{        
    protected override string TestDir => "../../ExsltTest/tests/EXSLT/Math/"; 
    protected override string ResultsDir => "../../ExsltTest/results/EXSLT/Math/"; 
                   
    /// <summary>
    /// Tests the following function:
    ///     math:min()
    /// </summary>
    [Fact]
    public void MinTest() 
    {
        RunAndCompare("source.xml", "min.xslt", "min.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:max()
    /// </summary>
    [Fact]
    public void MaxTest() 
    {
        RunAndCompare("source.xml", "max.xslt", "max.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:highest()
    /// </summary>
    [Fact]
    public void HighestTest() 
    {
        RunAndCompare("source.xml", "highest.xslt", "highest.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:lowest()
    /// </summary>
    [Fact]
    public void LowestTest() 
    {
        RunAndCompare("source.xml", "lowest.xslt", "lowest.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:abs()
    /// </summary>
    [Fact]
    public void AbsTest() 
    {
        RunAndCompare("source.xml", "abs.xslt", "abs.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:sqrt()
    /// </summary>
    [Fact]
    public void SqrtTest() 
    {
        RunAndCompare("source.xml", "sqrt.xslt", "sqrt.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:power()
    /// </summary>
    [Fact]
    public void PowerTest() 
    {
        RunAndCompare("source.xml", "power.xslt", "power.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:constant()
    /// </summary>
    [Fact]
    public void ConstantTest() 
    {
        RunAndCompare("source.xml", "constant.xslt", "constant.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:log()
    /// </summary>
    [Fact]
    public void LogTest() 
    {
        RunAndCompare("source.xml", "log.xslt", "log.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:random()
    /// </summary>
    [Fact]
    public void RandomTest() 
    {
        RunAndCompare("source.xml", "random.xslt", "random.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:sin()
    /// </summary>
    [Fact]
    public void SinTest() 
    {
        RunAndCompare("source.xml", "sin.xslt", "sin.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:cos()
    /// </summary>
    [Fact]
    public void CosTest() 
    {
        RunAndCompare("source.xml", "cos.xslt", "cos.xml");
    }            
    
    /// <summary>
    /// Tests the following function:
    ///     math:tan()
    /// </summary>
    [Fact]
    public void TanTest() 
    {
        RunAndCompare("source.xml", "tan.xslt", "tan.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:asin()
    /// </summary>
    [Fact]
    public void AsinTest() 
    {
        RunAndCompare("source.xml", "asin.xslt", "asin.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:acos()
    /// </summary>
    [Fact]
    public void AcosTest() 
    {
        RunAndCompare("source.xml", "acos.xslt", "acos.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:atan()
    /// </summary>
    [Fact]
    public void AtanTest() 
    {
        RunAndCompare("source.xml", "atan.xslt", "atan.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:atan2()
    /// </summary>
    [Fact]
    public void Atan2Test() 
    {
        RunAndCompare("source.xml", "atan2.xslt", "atan2.xml");
    }            

    /// <summary>
    /// Tests the following function:
    ///     math:exp()
    /// </summary>
    [Fact]
    public void ExpTest() 
    {
        RunAndCompare("source.xml", "exp.xslt", "exp.xml");
    }            
}
