Conforming to the principle
===========================

Let's take a look at how we can write our code so that it conforms to the single responsibility principle. We'll revert the changes we made previously, but reimplement them after we've refactored to see what sort of difference conforming to the principle makes.

We identified five responsibilities in the original design of our class:

1. Determining if a course can be ended.
2. Ending a course.
3. Determining if an issuance can be cancelled.
4. Cancelling issuances.
5. Logging errors.

It makes most sense that `MedicationCourseEnder` should only have the responsibility of ending courses, so let's split the other four responsibilities out of this class. Error logging is the simplest so we should start there.

## Splitting out error logging

First, we'll create a simple logger class that will now handle the responsibility of error logging. This class simply acts as a wrapper for the call to the static `Console.Log()` method. This is a good technique to allow functionality defined in external static methods to be passed into objects that require it.

```csharp
internal class ConsoleLogger : ILogger
{
    public void Log(String message) => Console.WriteLine(message);
}
```

We'll also define an interface to expose the logging functionality (note that our `ConsoleLogger` implements this interface). This will allow classes that require logging to reference the _interface_ rather than the concrete class, thus enabling us to easily swap out console logging for some other type of logging (show a dialog, log to a database etc.).

```csharp
public interface ILogger
{
    void Log(String message);
}
```

This is known as the [Strategy Pattern](https://www.dofactory.com/net/strategy-design-pattern), and we'll use it to decouple the extra responsibilities from our final design.

Now we can pass in a reference to an `ILogger` to the constructor of `MedicationCourseEnder`:

```diff
public class MedicationCourseEnder
{
+   private readonly ILogger _logger;

+   public MedicationCourseEnder(ILogger logger) => _logger = logger;

    public void End(MedicationCourse medicationCourse, String reasonForEnding)
    {
        if (medicationCourse.Status == CourseStatus.Active)
        {
            medicationCourse.Status = CourseStatus.Ended;
            medicationCourse.ReasonForEnding = reasonForEnding;
            foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active))
            {
                issuance.Status = IssuanceStatus.Cancelled;
                issuance.ReasonForCancelling = reasonForEnding;
            }
        }
        else
        {
-           Console.WriteLine($"{medicationCourse.PreparationTerm} course cannot be ended.");
+           _logger.Log($"{medicationCourse.PreparationTerm} course cannot be ended.");
            return;
        }
    }
}
```

Now, our `MedicationCourseEnder` isn't logging directly to the console, it's deferring that responsibility to whatever logger it was given.

The fact that we've used an interface gives us flexibility to pass in a completely different type of logger — this includes a mock logger that we can examine to assert that it was called when appropriate, and with the correct message.

We can now make the following change to our test class:

```diff
public class MedicationCourseEnderTests
{
	[Theory]
-	[InlineData(CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
-	[InlineData(CourseStatus.Active, IssuanceStatus.Cancelled, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, null)]
-	[InlineData(CourseStatus.Ended, IssuanceStatus.Active, CourseStatus.Ended, null, IssuanceStatus.Active, null)]
+	[InlineData(CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Ended, "Prescribing error", false, IssuanceStatus.Cancelled, "Prescribing error")]
+	[InlineData(CourseStatus.Active, IssuanceStatus.Cancelled, CourseStatus.Ended, "Prescribing error", false, IssuanceStatus.Cancelled, null)]
+	[InlineData(CourseStatus.Ended, IssuanceStatus.Active, CourseStatus.Ended, null, true, IssuanceStatus.Active, null)]
	public void EndTests(
		CourseStatus courseStatus,
		IssuanceStatus issuanceStatus,
		CourseStatus expectedCourseStatus,
		String expectedReasonForEnding,
+		Boolean expectedToLogError,
		IssuanceStatus expectedIssuanceStatus,
		String expectedReasonForCancelling)
	{
		var issuance = new Issuance(issuanceStatus);
		var course = new MedicationCourse("TestPreparation", courseStatus, new[] { issuance });
		var reasonForEnding = "Prescribing error";
+		var mockLogger = new Mock<ILogger>();

		new MedicationCourseEnder().End(course, reasonForEnding);
+		new MedicationCourseEnder(mockLogger.Object).End(course, reasonForEnding);

		Assert.Equal(expectedCourseStatus, course.Status);
		Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);
		Assert.Equal(expectedIssuanceStatus, issuance.Status);
		Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);

-		// Assert error was logged?
+		mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);
	}
}
```

## Splitting out issuance cancelling

Next, let's tackle the bigger responsibility of issuance cancellation. Again, we'll follow the strategy pattern to decouple this responsibility from the class.

As before, we'll create a concrete class to handle the behaviour we want to split out:

```csharp
public class MedicationIssuanceCanceller : IMedicationIssuanceCanceller
{
    public void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling)
    {
        foreach (var issuance in issuances.Where(i => i.Status == IssuanceStatus.Active))
        {
            issuance.Status = IssuanceStatus.Cancelled;
            issuance.ReasonForCancelling = reasonForCancelling;
        }
    }
}
```

We've simply cut the cancellation logic from the `MedicationCourseEnder` class and pasted it in here.

As before, we'll also create an interface to expose the `Cancel()` method:

```csharp
public interface IMedicationIssuanceCanceller
{
    void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling);
}
```

We could have made the `Cancel()` method take the whole `MedicationCourse` object as an argument. However, if we refer back to our scenario from earlier:

> Imagine we have another requirement that states we must allow the user to cancel specific issuances without ending a course.

Having our issue cancellation code automatically cancel _all_ issuances is semantically coupling our new code to the code that originally used it. Passing in the course object would not allow us to reuse this class to cancel specific issuances.

The way we've designed it allows us to call the method like so:

```csharp
Cancel(new[] { issuance1, issuance3, issuance7 }, "Prescribing error");
```

Now we can follow the strategy pattern as before and pass in a reference to our new canceller interface. Then simply call the `Cancel()` method on whichever type of canceller we have:

```diff
public class MedicationCourseEnder
{
    private readonly ILogger _logger;
+   private readonly IMedicationIssuanceCanceller _issuanceCanceller;

-   public MedicationCourseEnder(ILogger logger) => _logger = logger;
+   public MedicationCourseEnder(ILogger logger, IMedicationIssuanceCanceller issuanceCanceller)
+   {
+       _logger = logger;
+       _issuanceCanceller = issuanceCanceller;
+   }

    public void End(MedicationCourse medicationCourse, String reasonForEnding)
    {
        if (medicationCourse.Status == CourseStatus.Active)
        {
            medicationCourse.Status = CourseStatus.Ended;
            medicationCourse.ReasonForEnding = reasonForEnding;

-           foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active))
-           {
-               issuance.Status = IssuanceStatus.Cancelled;
-               issuance.ReasonForCancelling = reasonForEnding;
-           }
+           _issuanceCanceller.Cancel(medicationCourse.Issuances, reasonForEnding);
        }
        else
        {
            _logger.Log($"{medicationCourse.PreparationTerm} course cannot be ended.");
            return;
        }
    }
}
```

Now we can make similar changes to our test class as we did when splitting out the logging functionality, but we can also simplify the test data for our existing test class. Here's how our test class looks now. We'll look at the whole class rather than the diff, as there are many changes.

```csharp
public class MedicationCourseEnderTests
{
    [Theory]
    [InlineData(CourseStatus.Active, CourseStatus.Ended, "Prescribing error", false, true)]
    [InlineData(CourseStatus.Ended, CourseStatus.Ended, null, true, false)]
    public void EndTests(
        CourseStatus status,
        CourseStatus expectedStatus,
        String expectedReasonForEnding,
        Boolean expectedToLogError,
        Boolean expectedToCancelIssuances)
    {
        var issuances = Enumerable.Empty<Issuance>();
        var course = new MedicationCourse("TestPreparation", status, issuances);
        var reasonForEnding = "Prescribing error";
        var mockLogger = new Mock<ILogger>();
        var mockIssuanceCanceller = new Mock<IMedicationIssuanceCanceller>();

        new MedicationCourseEnder(mockLogger.Object, mockIssuanceCanceller.Object).End(course, reasonForEnding);

        Assert.Equal(expectedStatus, course.Status);
        Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);

        mockLogger.Verify(mock => mock.Log(It.Is<String>(match => match == "TestPreparation course cannot be ended.")), expectedToLogError ? Times.Once : Times.Never);

        mockIssuanceCanceller.Verify(mock =>
            mock.Cancel(
                It.Is<IEnumerable<Issuance>>(match => match == issuances),
                It.Is<String>(match => match == reasonForEnding)),
            expectedToCancelIssuances ? Times.Once : Times.Never);
    }
}
```

Now that we're only concerned with the ending of courses, we've removed any detail on what happens to the issuance objects. We state whether we expect the course to end, and check that the correct arguments were supplied to either the logger or the issue canceller. We don't need to muddy the tests by asserting whatever issuance cancellation logic should be applied.

Obviously, we now need to test our `MedicationIssuanceCanceller` class separately:

```csharp
public class MedicationIssuanceCancellerTests
{
    [Theory]
    [InlineData(IssuanceStatus.Active, true)]
    [InlineData(IssuanceStatus.Cancelled, false)]
    public void CancelTests(IssuanceStatus status, Boolean expectedToCancel)
    {
        var issuance = new Issuance(status);
        var reasonForCancelling = "Prescribing error";

        new MedicationIssuanceCanceller().Cancel(new[] { issuance }, reasonForCancelling);

        Assert.Equal(expectedToCancel ? IssuanceStatus.Cancelled : status, issuance.Status);
        Assert.Equal(expectedToCancel ? reasonForCancelling : null, issuance.ReasonForCancelling);
    }
}
```

So we've ended up with slightly more code and one more test, but if we look at the truth tables that now represent our tests, you'll see that this is much easier to understand at a glance:

`MedicationCourseEnder.cs`
| **Status** | **Expected status** | **Expected reason for ending** | **Expected to cancel issuances** | **Expected to log error** |
|:-----------|:--------------------|:-------------------------------|:--------------------------------:|:-------------------------:|
| Active     | Ended               | "Prescribing error"            | ✅                               | ❌                        |
| Ended      | Ended               | [Empty]                        | ❌                               | ✅                        |

`MedicationIssuanceCanceller.cs`
| **Status**    | **Expected status** | **Expected reason for cancelling** |
|:--------------|:--------------------|:-----------------------------------|
| Active        | Cancelled           | "Prescribing error"                |
| Cancelled     | Cancelled           | [Empty]                            |

Another subtle improvement here that's worth calling out is the readability of the finished product. Now that each class is responsible for only one thing, we've gone from having to reference `courseStatus` and `issuanceStatus` to simply `status` in each test class.

This is an improvement on cognitive load, and can help massively when the logic is more complex than in this simple example.

## Splitting out medication course selection

Next up is the splitting of the course selection functionality. We'll be using the same strategy pattern but, this time, let's start from the `MedicationCourseEnder.cs` class.

```diff
-	public MedicationCourseEnder(ILogger logger, IMedicationIssuanceCanceller issuanceCanceller)
+	public MedicationCourseEnder(ILogger logger, IMedicationIssuanceCanceller issuanceCanceller, IMedicationCourseSelector selector)
	{
		_logger = logger;
		_issuanceCanceller = issuanceCanceller;
+		_selector = selector;
	}

	public void End(MedicationCourse medicationCourse, String reasonForEnding)
	{
-		if (medicationCourse.Status == CourseStatus.Active)
+		if (_selector.ShouldEnd(medicationCourse))
		{
```

We've replaced the part of the `End()` method, that determines whether a course can be ended, with a call to a `ShouldEnd()` method on an `IMedicationCourseSelector` interface. We'll move this logic into a concrete class that implements this interface:

```csharp
public class ActiveCoursesSelector : IMedicationCourseSelector
{
    public Boolean ShouldEnd(MedicationCourse medicationCourse) => medicationCourse.Status == CourseStatus.Active;
}
```

```csharp
public interface IMedicationCourseSelector
{
    Boolean ShouldEnd(MedicationCourse medicationCourse);
}
```

Next we have to update our existing tests for the `MedicationCourseEnder` class:

```diff
public class MedicationCourseEnderTests
{
	[Theory]
-	[InlineData(CourseStatus.Active, CourseStatus.Ended, "Prescribing error", false, true)]
-	[InlineData(CourseStatus.Ended, CourseStatus.Ended, null, true, false)]
+	[InlineData(true, CourseStatus.Ended, "Prescribing error", false, true)]
+	[InlineData(false, CourseStatus.Active, null, true, false)]
	public void EndTests(
-		CourseStatus status,
+		Boolean shouldEnd,
		CourseStatus expectedStatus,
		String expectedReasonForEnding,
		Boolean expectedToLogError,
		Boolean expectedToCancelIssuances)
	{
		var issuances = Enumerable.Empty<Issuance>();
-		var course = new MedicationCourse("TestPreparation", status, issuances);
+		var course = new MedicationCourse("TestPreparation", CourseStatus.Active, issuances);
		var reasonForEnding = "Prescribing error";
		var mockLogger = new Mock<ILogger>();
		var mockIssuanceCanceller = new Mock<IMedicationIssuanceCanceller>();
+		var mockSelector = new Mock<IMedicationCourseSelector>();
+		mockSelector.Setup(mock => mock.ShouldEnd(It.IsAny<MedicationCourse>())).Returns(shouldEnd);

-		new MedicationCourseEnder(mockLogger.Object, mockIssuanceCanceller.Object).End(course, reasonForEnding);
+		new MedicationCourseEnder(mockLogger.Object, mockIssuanceCanceller.Object, mockSelector.Object).End(course, reasonForEnding);

		Assert.Equal(expectedStatus, course.Status);
		Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);

+		mockSelector.Verify(mock => mock.ShouldEnd(It.Is<MedicationCourse>(match => match == course)));
```
Here, we've replaced the only input we had (`status`), with a simple boolean. The `MedicationCourse` is given an active status, but this is simply to satisfy the constructor, and could have been any status, since our selection logic is now irrelevant to the `MedicationCourseEnder` class.

We then set up a mock selector and have it return our `shouldEnd` boolean. Finally, we verify that the course object was passed correctly to our mock selector.

This doesn't really simplify the test at this point but, as we covered earlier in the session, the principle really begins to reap rewards once we implement change. We'll see those rewards after we've finished all our refactoring.

Of course, we still need to test that our system only allows the ending of active courses. This is simply a case of writing two small tests for our concrete selector class:

```csharp
public class ActiveCoursesSelectorTests
{
    [Theory]
    [InlineData(CourseStatus.Active, true)]
    [InlineData(CourseStatus.Ended, false)]
    public void ShouldEndTests(CourseStatus status, Boolean expectedShouldEnd) =>
        Assert.Equal(
            expectedShouldEnd,
            new ActiveCoursesSelector()
                .ShouldEnd(
                    new MedicationCourse(
                        preparationTerm: String.Empty,
                        status: status,
                        issuances: Enumerable.Empty<Issuance>())));
}
```

In an ideal world, our `ShouldEnd` method would take some interface as an argument rather than our concrete `MedicationCourse` class but, for the sake of clarity, we'll leave this as is.

## Splitting out issuance selection

Finally, the last responsibility we need to split out is the selection of issuances to cancel. This is pretty much the same change as the previous one, so the description here will be left sparse.

Replace the hardcoded selection statement with a call to a method on a selector interface:

```diff
public class MedicationIssuanceCanceller : IMedicationIssuanceCanceller
{
+	private readonly IMedicationIssuanceSelector _selector;

+	public MedicationIssuanceCanceller(IMedicationIssuanceSelector selector) => _selector = selector;

	public void Cancel(IEnumerable<Issuance> issuances, String reasonForCancelling)
	{
-		foreach (var issuance in issuances.Where(i => i.Status == IssuanceStatus.Active))
+		foreach (var issuance in issuances.Where(_selector.ShouldCancel))
		{
```

Add the new selector class and interface:

```csharp
public class ActiveIssuancesSelector : IMedicationIssuanceSelector
{
    public Boolean ShouldCancel(Issuance issuance) => issuance.Status == IssuanceStatus.Active;
}
```

```csharp
public interface IMedicationIssuanceSelector
{
    Boolean ShouldCancel(Issuance issuance);
}
```

Update the existing tests:

```diff
public class MedicationIssuanceCancellerTests
{
	[Theory]
-	[InlineData(IssuanceStatus.Active, IssuanceStatus.Cancelled, "Prescribing error")]
-	[InlineData(IssuanceStatus.Cancelled, IssuanceStatus.Cancelled, null)]
-	public void CancelTests(IssuanceStatus status, IssuanceStatus expectedStatus, String expectedReasonForCancelling)
+	[InlineData(true, IssuanceStatus.Cancelled, "Prescribing error")]
+	[InlineData(false, IssuanceStatus.Active, null)]
+	public void CancelTests(Boolean shouldCancel, IssuanceStatus expectedStatus, String expectedReasonForCancelling)
	{
-		var issuance = new Issuance(status);
+		var issuance = new Issuance(IssuanceStatus.Active);
		var reasonForCancelling = "Prescribing error";

-		new MedicationIssuanceCanceller().Cancel(new[] { issuance }, reasonForCancelling);
+		var mockSelector = new Mock<IMedicationIssuanceSelector>();
+		mockSelector.Setup(mock => mock.ShouldCancel(It.IsAny<Issuance>())).Returns(shouldCancel);

+		new MedicationIssuanceCanceller(mockSelector.Object).Cancel(new[] { issuance }, reasonForCancelling);

		Assert.Equal(expectedStatus, issuance.Status);
		Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);
+		mockSelector.Verify(mock => mock.ShouldCancel(It.Is<Issuance>(match => match == issuance)));
	}
}
```

Add tests for the new selector class:

```csharp
public class ActiveIssuancesSelectorTests
{
    [Theory]
    [InlineData(IssuanceStatus.Active, true)]
    [InlineData(IssuanceStatus.Cancelled, false)]
    public void ShouldCancelTests(IssuanceStatus status, Boolean expectedShouldCancel) =>
        Assert.Equal(
            expectedShouldCancel,
            new ActiveIssuancesSelector()
                .ShouldCancel(
                    new Issuance(status)));
}
```

Now that all of our refactoring is complete, we can see what improvements it's made by [reapplying the changes to requirements that we did previously](./Readme-reapplying-changes.md)...