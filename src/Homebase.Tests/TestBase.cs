using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace Homebase.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        protected Exception Exception { get; private set; }
        protected bool IsExceptionExpected { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            try
            {
                Arrange();
                Act();  
            }
            catch (Exception ex) when (IsExceptionExpected)
            {
                Exception = ex;
            }
            finally
            {
                if (IsExceptionExpected)
                    Assert.IsNotNull(Exception);
            }
        }

        protected virtual void Arrange()
        {
        }

        protected virtual void Act()
        {
        }
    }
}
