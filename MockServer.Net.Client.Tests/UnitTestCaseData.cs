namespace MockServer.Net.Client.Tests
{
    using System.Collections;
    using NUnit.Framework;

    public class UnitTestCaseData
    {
        public static IEnumerable SingleArgumentOkTestData
        {
            get
            {
                yield return new TestCaseData("Expectation", 201, "expectation created", string.Empty)
                     .SetName("CreateExpectation_ShouldReturnCreated")
                     .SetCategory("Expectation");
                yield return new TestCaseData("Expectation", 400, "incorrect request format", string.Empty)
                    .SetName("CreateExpectation_ShouldReturnBadRequest")
                    .SetCategory("Expectation");
                yield return new TestCaseData("Expectation", 406, "invalid expectation", string.Empty)
                    .SetName("CreateExpectation_ShouldReturnNotAcceptable")
                    .SetCategory("Expectation");
                yield return new TestCaseData("Verify", 200, "matching request has been received specified number of times", string.Empty)
                    .SetName("VerifyRequest_ShouldReturnOk")
                    .SetCategory("Verify");
                yield return new TestCaseData("Verify", 400, "incorrect request format", string.Empty)
                    .SetName("VerifyRequest_ShouldReturnBadRequest")
                    .SetCategory("Verify");
                yield return new TestCaseData("Verify", 406, "request has not been received specified numbers of times", string.Empty)
                    .SetName("VerifyRequest_ShouldReturnNotAcceptable")
                    .SetCategory("Verify");
                yield return new TestCaseData("VerifySequence", 202, "request sequence has been received in specified order", string.Empty)
                    .SetName("VerifySequenceRequest_ShouldReturnAccepted")
                    .SetCategory("VerifySequence");
                yield return new TestCaseData("VerifySequence", 400, "request has not been received specified numbers of times", string.Empty)
                    .SetName("VerifySequenceRequest_ShouldReturnBadRequest")
                    .SetCategory("VerifySequence");
                yield return new TestCaseData("VerifySequence", 406, "request has not been received specified numbers of times", string.Empty)
                    .SetName("VerifySequenceRequest_ShouldReturnNotAcceptable")
                    .SetCategory("VerifySequence");
                yield return new TestCaseData("Clear", 200, "expectations and recorded requests cleared", string.Empty)
                    .SetName("ClearRequest_ShouldReturnOk")
                    .SetCategory("Clear");
                yield return new TestCaseData("Clear", 400, "incorrect request format", string.Empty)
                    .SetName("ClearRequest_ShouldReturnBadRequest")
                    .SetCategory("Clear");
                yield return new TestCaseData("Bind", 200, "listening on additional requested ports, note: the response ony contains ports added for the request, to list all ports use /status", string.Empty)
                    .SetName("BindRequest_ShouldReturnOk")
                    .SetCategory("Bind");
                yield return new TestCaseData("Bind", 400, "incorrect request format", string.Empty)
                    .SetName("BindRequest_ShouldReturnBadRequest")
                    .SetCategory("Bind");
                yield return new TestCaseData("Bind", 406, "unable to bind to ports(i.e.already bound or JVM process doesn't have permission)", string.Empty)
                    .SetName("BindRequest_ShouldReturnNotAcceptable")
                    .SetCategory("Bind");
            }
        }

        public static IEnumerable OkTestData
        {
            get
            {
                yield return new TestCaseData("Reset", 200, "expectations and recorded requests cleared", string.Empty)
                    .SetName("ResetRequest_ShouldReturnOk")
                    .SetCategory("Reset");
                yield return new TestCaseData("Retrieve", 200, "recorded requests or active expectations returned", string.Empty)
                    .SetName("RetrieveRequest_ShouldReturnOk")
                    .SetCategory("Retrieve");
                yield return new TestCaseData("Retrieve", 400, "incorrect request format", string.Empty)
                    .SetName("RetrieveRequest_ShouldReturBadRequest")
                    .SetCategory("Retrieve");
                yield return new TestCaseData("Status", 200, "MockServer is running and listening on the listed ports", string.Empty)
                    .SetName("StatusRequest_ShouldReturnOk")
                    .SetCategory("Status");
                yield return new TestCaseData("Stop", 200, "MockServer process is stopping", string.Empty)
                    .SetName("StopRequest_ShouldReturnOk")
                    .SetCategory("Stop");
            }
        }

        public static IEnumerable ExceptionTestData
        {
            get
            {
                yield return new TestCaseData("Reset")
                    .SetName("ResetRequest_ShouldThrowException")
                    .SetCategory("Reset");
                yield return new TestCaseData("Retrieve")
                    .SetName("RetrieveRequest_ShouldThrowException")
                    .SetCategory("Retrieve");
                yield return new TestCaseData("Status")
                    .SetName("StatusRequest_ShouldThrowException")
                    .SetCategory("Status");
                yield return new TestCaseData("Stop")
                    .SetName("StopRequest_ShouldThrowException")
                    .SetCategory("Stop");
            }
        }

        public static IEnumerable SingleArgumentExceptionTestData
        {
            get
            {
                yield return new TestCaseData("Expectation")
                    .SetName("CreateExpectation_ShouldThrowException")
                    .SetCategory("Expectation");
                yield return new TestCaseData("Verify")
                    .SetName("VerifyRequest_ShouldThrowException")
                    .SetCategory("Verify");
                yield return new TestCaseData("VerifySequence")
                    .SetName("VerifySequenceRequest_ShouldThrowException")
                    .SetCategory("VerifySequence");
                yield return new TestCaseData("Clear")
                    .SetName("ClearRequest_ShouldThrowException")
                    .SetCategory("Clear");
                yield return new TestCaseData("Bind")
                    .SetName("BindRequest_ShouldThrowException")
                    .SetCategory("Bind");
            }
        }
    }
}