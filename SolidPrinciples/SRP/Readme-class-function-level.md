Applying the SRP at the class/function level (an in-depth case study)
============================================

Let's take a more in-depth look at this principle in practice, and discuss how the principle is applied at a class/function level.

---
**NOTE:** The implementation of the solution discussed in this case study can be found [in this pull request](https://github.com/marshallandy83/code-quality-sessions/pull/1). This includes the full commit history of the changes made in each step.

---

Imagine we've been given a task to implement some functionality to the medication module of the same system we looked at in the previous example. We've been given the following acceptance criteria:

* As a user, I want the ability to end an _active_ medication course.
* Any _active_ issuances of the course should also be cancelled.

## Implementation (not conforming to principle)

Let's look to implement this feature in the simplest way possible. We might throw together something like this:

```csharp
public class MedicationCourseEnder
{
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
            Console.WriteLine($"{medicationCourse.PreparationTerm} course cannot be ended.");
            return;
        }
    }
}
```

## Non-conformity

This satisfies our acceptance criteria, but it doesn't conform to the single responsibility principle.

A good way of verifying whether an area of our system conforms to the single responsibility principle is to try to describe what it does. A good description for this class would be:

> The MedicationCourseEnder ends the course and cancels any active issuances.

The inclusion of the word "and" in the description is a good indicator that we're doing more than one thing.

So it's fairly easy to determine that we're not just ending the course, we're also cancelling issuances. But there are actually more responsibilities that might not be as immediately obvious.

Firstly: we're also logging to the console if the course cannot be ended. This is a good example of something that you might not consider to be a separate responsibility. But if we take another look at what the principle states:

> A class should have one, and only one, reason to change.

It's not out of the question that there may be some future requirement that asks us to, instead, show a dialog to the user; or log to a monitoring database; or even silently return. It's this fact that tells us that we can be quite justified in considering this to be a completely separate responsibility — because it's a potential reason to change.

The other hidden responsibilities are more difficult to find. The `MedicationCourseEnder` class is performing actions on the medication objects (ending the course/cancelling issuances) but, importantly, it's also determining whether or not those actions should be performed.

There is certainly the possibility that we'd want to change the logic that determines if a course can be ended; and there's also the possibility that we'd want to change what happens when we end the course. And the same goes for issuance cancellation.

Again, it's these multiple "reasons to change" that tell us we're not conforming to the single responsibility principle.

Let's have another look at the code and highlight the different responsibilities:

<p align="center">
  <img src="./images/code-highlighted-responsibilities.png?raw=true"/>
</p>

One thing you may be questioning is why we don't consider updating the course/issuance status and setting the reason for ending/cancelling as separate responsibilities. This, again, goes back to identifying the "reasons to change". It's easy to envisage adding another property change to one of these responsibilities — perhaps logging the user that performed the action for example. But would we say that we'd change one of the individual lines of code? What would we set the status to instead of what we've already got? What would we store as the reason for ending/cancelling? It could be strongly argued that it's much less likely for either of these things to change, therefore, we wouldn't consider these separate responsibilities.

So, to summarise, the `MedicationCourseEnder` class has the following five responsibilities:

1. Determining if a course can be ended.
2. Ending a course.
3. Determining if an issuance can be cancelled.
4. Cancelling issuances.
5. Logging errors.

## Issues that may arise

### Testability

Most of the issues that result from not following the single responsibility principle (or any of the principles for that matter) occur when there's a need to change our code. However, this solution has presented us with one problem immediately.

Let's get some tests together to verify our method is working correctly:

```csharp
public class MedicationCourseEnderTests
{
	[Theory]
	[InlineData(CourseStatus.Active, IssuanceStatus.Active, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, "Prescribing error")]
	[InlineData(CourseStatus.Active, IssuanceStatus.Cancelled, CourseStatus.Ended, "Prescribing error", IssuanceStatus.Cancelled, null)]
	[InlineData(CourseStatus.Ended, IssuanceStatus.Active, CourseStatus.Ended, null, IssuanceStatus.Active, null)]
	public void EndTests(
		CourseStatus courseStatus,
		IssuanceStatus issuanceStatus,
		CourseStatus expectedCourseStatus,
		String expectedReasonForEnding,
		IssuanceStatus expectedIssuanceStatus,
		String expectedReasonForCancelling)
	{
		var issuance = new Issuance(issuanceStatus);
		var course = new MedicationCourse("TestPreparation", courseStatus, new[] { issuance });
		var reasonForEnding = "Prescribing error";

		new MedicationCourseEnder().End(course, reasonForEnding);

		Assert.Equal(expectedCourseStatus, course.Status);
		Assert.Equal(expectedReasonForEnding, course.ReasonForEnding);
		Assert.Equal(expectedIssuanceStatus, issuance.Status);
		Assert.Equal(expectedReasonForCancelling, issuance.ReasonForCancelling);

		// Assert error was logged?
	}
}
```

We have a problem. To fully test this class, we must test that the correct error is logged.

Since we're logging to the console directly within the `MedicationCourseEnder` class, we can't easily test that the correct message was logged.

---
**NOTE:** It is actually possible to mock the console, but for the purposes of this simple demonstration, we'll consider this console log to be some other non-mockable static method call.

---

To look at the next testability issues that may arise, we need to introduce change. It's the fact that the products we create _will_ change that means it's so important to produce a clean design from the very start.

Earlier on, we identified that this class has five "reasons to change". We'd need to change this class if there was:

1. A change to the course-ending eligibility.
2. A change to the way courses are ended.
3. A change to the issuance-cancellation eligibility.
4. A change to the way issuances are cancelled.
5. A change to the way errors are communicated.

Let's introduce change and see how this affects our testability.

Currently, to fully test our class, we have three scenarios to consider:

| **Course status** | **Issuance status** | **Expected course status** | **Expected reason for ending** | **Expected issuance status** | **Expected reason for cancelling** | **Expected to log error** |
|:------------------|:--------------------|:---------------------------|:-------------------------------|:-----------------------------|:-----------------------------------|:-------------------------:|
| Active            | Active              | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Active            | Cancelled           | Ended                      | "Prescribing error"            | Cancelled                    | [Empty]                            | ❌                        |
| Ended             | Active              | Ended                      | [Empty]                        | Active                       | [Empty]                            | ✅                        |

Having two input parameters and five assertions to make is already muddying the waters, but this will get worse as we implement new requirements...

Let's imagine the customer has approached us with a change to the course-ending eligibility requirements (responsibility 1):

* As a user, I want the ability to determine whether a course was added locally or externally.
* It should only be possible to end a _local_ medication course.

To satisfy the change to requirements, we might make the following code changes:

`MedicationCourse.cs`
```diff
public class MedicationCourse
{
    public MedicationCourse(
        String preparationTerm,
        CourseStatus status,
-       IEnumerable<Issuance> issuances)
+       IEnumerable<Issuance> issuances,
+       Source addedBy)
    {
        PreparationTerm = preparationTerm;
        Status = status;
        Issuances = issuances;
+       AddedBy = addedBy;
    }

    public String PreparationTerm { get; }
    public CourseStatus Status { get; set; }
    public IEnumerable<Issuance> Issuances { get; }
    public String ReasonForEnding { get; set; }
+   public Source AddedBy { get; }
}
```
`MedicationCourseEnder.cs`
```diff
public void End(MedicationCourse medicationCourse, String reasonForEnding)
{
-   if (medicationCourse.Status == CourseStatus.Active)
+   if (medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local)
    {
        medicationCourse.Status = CourseStatus.Ended;
        medicationCourse.ReasonForEnding = reasonForEnding;
```

After this fairly minor change, our test scenarios now look like this:

| **Course added by** | **Course status** | **Issuance status** | **Expected course status** | **Expected reason for ending** | **Expected issuance status** | **Expected reason for cancelling** | **Expected to log error** |
|:--------------------|:------------------|:--------------------|:---------------------------|:-------------------------------|:-----------------------------|:-----------------------------------|:-------------------------:|
| Local               | Active            | Active              | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | Cancelled           | Ended                      | "Prescribing error"            | Cancelled                    | [Empty]                            | ❌                        |
| Local               | Ended             | Active              | Ended                      | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| External            | Active            | Active              | Active                     | [Empty]                        | Active                       | [Empty]                            | ✅                        |

Now let's imagine the customer has decided they'd like a similar change to the issuance-cancellation eligibility requirements(responsibility 3):

* As a user, I want the ability to determine whether an issuance was added locally or externally.
* It should only be possible to cancel a _local_ medication issuance.

To satisfy these requirements, we'd probably make similar changes to before:

`Issuance.cs`
```diff
public class Issuance
{
-   public Issuance(IssuanceStatus status) => Status = status;
+   public Issuance(IssuanceStatus status, Source addedBy)
+   {
+       Status = status;
+       AddedBy = addedBy;
+   }

    public IssuanceStatus Status { get; set; }
    public String ReasonForCancelling { get; set; }
+   public Source AddedBy { get; }
}
```

`MedicationCourseEnder.cs`
```diff
if (medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local)
{
    medicationCourse.Status = CourseStatus.Ended;
    medicationCourse.ReasonForEnding = reasonForEnding;

-   foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active))
+   foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active && i.AddedBy == Source.Local))
    {
        issuance.Status = IssuanceStatus.Cancelled;
        issuance.ReasonForCancelling = reasonForEnding;
```

After our second change, our test scenarios now look like this:

| **Course added by** | **Course status** | **Issuance added by** | **Issuance status** | **Expected course status** | **Expected reason for ending** | **Expected issuance status** | **Expected reason for cancelling** | **Expected to log error** |
|:--------------------|:------------------|:----------------------|:--------------------|:---------------------------|:-------------------------------|:-----------------------------|:-----------------------------------|:-------------------------:|
| Local               | Active            | Local                 | Active              | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | Local                 | Cancelled           | Ended                      | "Prescribing error"            | Cancelled                    | [Empty]                            | ❌                        |
| Local               | Ended             | Local                 | Active              | Ended                      | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| External            | Active            | Local                 | Active              | Active                     | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| Local               | Active            | External              | Active              | Ended                      | "Prescribing error"            | Active                       | [Empty]                            | ❌                        |

As you can see, each minor change we make to our class increases the test complexity by more each time. We've got to change each existing test so that it satisfies the new condition, before adding a new test to _test_ the new condition. As a result, it's becoming less and less easy to see, at a glance, how the method should behave.

### Cognitive load

Another issue that non-conformity can cause is an increase in cognitive load. If we take the above truth table as a depiction of the multiple branches that a developer must hold in their head during a debugging session, we can see that this is needlessly complex.

This results in an increase in the time taken to debug issues, and an increase in the possibility of introducing further bugs.

### Reusability

Just like in our first example at the module level, violation of this principle results in our code being less reusable.

Imagine we have another requirement that states we must allow the user to cancel specific issuances without ending a course. Or what if another customer would like the ability to end a course without cancelling any issuances?

The fact that course ending and issuance cancellation are so tightly coupled means that we'd have to refactor our existing code in order to satisfy these requirements. This would require retesting; implementing a change to the course-ending functionality would require us to retest issuance-cancellation code; and vice versa.

### Flexibility

The tight-coupling of our courses code to our issuances code has a detrimental effect on reusability; but there's another example of tight coupling that affects the _flexibility_ of our code.

The fact that course/issuance _selection_ is tightly coupled to the code that ends/cancels, means that it's difficult for us to offer flexibility.

Let's look at another possible change in requirements. Let's imagine that our software becomes popular and we start to offer different configurations for different customers.

* As a user from Customer 2, I want the ability to end/cancel external courses/issuances.

The new customer doesn't want the same restrictions on external sources that we implemented for our existing customer.

Of course, we still need to keep the current functionality in place for our existing customer, so we need to change our course/issuance selection only in certain circumstances.

We might do something simple like passing in a boolean flag to the method:

```diff
public class MedicationCourseEnder
{
-	public void End(MedicationCourse medicationCourse, String reasonForEnding)
+	public void End(MedicationCourse medicationCourse, String reasonForEnding, Boolean allowExternalEnding)
	{
-		if (medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local)
+		if (medicationCourse.Status == CourseStatus.Active && (medicationCourse.AddedBy == Source.Local || allowExternalEnding))
		{
			medicationCourse.Status = CourseStatus.Ended;
			medicationCourse.ReasonForEnding = reasonForEnding;
			
-			foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active && i.AddedBy == Source.Local))
+			foreach (var issuance in medicationCourse.Issuances.Where(i => i.Status == IssuanceStatus.Active && (i.AddedBy == Source.Local || allowExternalEnding)))
			{
				issuance.Status = IssuanceStatus.Cancelled;
				issuance.ReasonForCancelling = reasonForEnding;
			}
```

And once again, we'll need to add new tests to cover these scenarios:

| **Course added by** | **Course status** | **Issuance added by** | **Issuance status** | **Allow external ending** | **Expected course status** | **Expected reason for ending** | **Expected issuance status** | **Expected reason for cancelling** | **Expected to log error** |
|:--------------------|:------------------|:----------------------|:--------------------|:-------------------------:|:---------------------------|:-------------------------------|:-----------------------------|:-----------------------------------|:-------------------------:|
| Local               | Active            | Local                 | Active              | ❌                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | Local                 | Cancelled           | ❌                        | Ended                      | "Prescribing error"            | Cancelled                    | [Empty]                            | ❌                        |
| Local               | Ended             | Local                 | Active              | ❌                        | Ended                      | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| External            | Active            | Local                 | Active              | ❌                        | Active                     | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| Local               | Active            | External              | Active              | ❌                        | Ended                      | "Prescribing error"            | Active                       | [Empty]                            | ❌                        |
| External            | Active            | Local                 | Active              | ✅                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | External              | Active              | ✅                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |


As you can see, making this relatively straightforward change adds complexity to both our code and our tests. This is because our original code was not designed to be flexible. And this simple example is just one way that we might need to change our existing design to fit in requirements from a new customer. A few years in the future we could have something like this:

```csharp
public void End(
    MedicationCourse medicationCourse,
    String reasonForEnding,
    Boolean allowExternalEnding,
    Boolean allowCancellationOfIssuedCourses,
    Boolean requiresAuthorisation)
{
    if (medicationCourse.Status == CourseStatus.Active &&
        (medicationCourse.AddedBy == Source.Local || allowExternalEnding) &&
        (medicationCourse.PrescriptionType != PrescriptionType.PrescribedElsewhere) &&
        (!medicationCourse.Issuances.Any(issuance => issuance.Status == IssuanceStatus.Issued || allowCancellationOfIssuedCourses)) &&
        (medicationCourse.Authoriser != null || !requiresAuthorisation))
    {
```

Would you like to try to find a bug in the above code? Or add a new condition without breaking the existing functionality? I wouldn't!

Passing in a boolean flag to a method or class constructor is another good indicator that your method/class is violating the single responsibility principle. The inclusion of a boolean flag is really saying that your method/class is trying to be two different things. There's normally a better way to achieve this, i.e. have two separate methods/classes with just one responsibility!

It should also be pointed out that designing an inflexible system like this also has a negative effect on testability. If we have a suite of manual tests to run against our software, the tests for the existing functionality would have to be re-run after making changes like this.

Next up, we'll revert these changes and refactor our existing code so that it [conforms to the single responsibility principle](./Readme-conforming.md)...