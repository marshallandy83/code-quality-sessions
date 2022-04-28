Reapplying the changes
======================

Now that we've refactored our existing code to conform to the single responsibility principle, we can make the same changes we did earlier and see what benefits we get.

## Introducing the source filtering requirements

What's the best way to implement the source filtering requirements?

Requirement 1
* As a user, I want the ability to determine whether a course was added locally or externally.
* It should only be possible to end a _local_ medication course.

Requirement 2
* As a user, I want the ability to determine whether an issuance was added locally or externally.
* It should only be possible to cancel a _local_ medication issuance.

We could make similar changes to the ones we did before we refactored the code:

```diff
public class ActiveCoursesSelector : IMedicationCourseSelector
{
-	public Boolean ShouldEnd(MedicationCourse medicationCourse) => medicationCourse.Status == CourseStatus.Active;
+	public Boolean ShouldEnd(MedicationCourse medicationCourse) => medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local;
}

public class ActiveIssuancesSelector : IMedicationIssuanceSelector
{
-	public Boolean ShouldCancel(Issuance issuance) => issuance.Status == IssuanceStatus.Active;
+	public Boolean ShouldCancel(Issuance issuance) => issuance.Status == IssuanceStatus.Active && issuance.AddedBy == Source.Local;
}
```

But if we look ahead and think about the scenario where another customer doesn't want these requirements, there's actually a better way to implement this.

## Introducing the "new customer" requirement

We can take advantage of the strategy pattern we implemented and, instead, add two new selectors that will handle the source filtering:

```csharp
public class ActiveAndLocalCoursesSelector : IMedicationCourseSelector
{
	public Boolean ShouldEnd(MedicationCourse medicationCourse) =>
		medicationCourse.Status == CourseStatus.Active && medicationCourse.AddedBy == Source.Local;
}

public class ActiveAndLocalIssuancesSelector : IMedicationIssuanceSelector
{
	public Boolean ShouldCancel(Issuance issuance) =>
		issuance.Status == IssuanceStatus.Active && issuance.AddedBy == Source.Local;
}
```

Now, all we have to do when we instantiate the `MedicationCourseEnder` class is to pass in whichever selector we want based on which customer we're serving. We could do something like this:

```csharp
static void Main(String[] args)
{
    Int32 customerNumber = Int32.Parse(args[0]);

    var courseEnder = new MedicationCourseEnder(
        new ConsoleLogger(),
        new MedicationIssuanceCanceller(
            customerNumber == 1 ? new ActiveAndLocalIssuancesSelector() : new ActiveIssuancesSelector()),
        customerNumber == 1 ? new ActiveAndLocalCoursesSelector() : new ActiveCoursesSelector());
}
```

The beauty of this approach is that, not only does it make the code easier to understand and test, but implementing these new requirements **does not change the existing code**. This means there's no danger of breaking an existing customer's implementation. In fact, there's no need to even run any manual tests to verify this either!

We'll go much deeper into this concept in the next session on the open/closed principle.

## Assessing the difference

Now that our code fully conforms to the single responsibility principle, we can have another look at the issues we raised earlier and see what difference we've made to them.

### Testability

Our code has become much easier to test. We've gone from one large, unwieldy set of tests...

| **Course added by** | **Course status** | **Issuance added by** | **Issuance status** | **Allow external ending** | **Expected course status** | **Expected reason for ending** | **Expected issuance status** | **Expected reason for cancelling** | **Expected to log error** |
|:--------------------|:------------------|:----------------------|:--------------------|:-------------------------:|:---------------------------|:-------------------------------|:-----------------------------|:-----------------------------------|:-------------------------:|
| Local               | Active            | Local                 | Active              | ❌                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | Local                 | Cancelled           | ❌                        | Ended                      | "Prescribing error"            | Cancelled                    | [Empty]                            | ❌                        |
| Local               | Ended             | Local                 | Active              | ❌                        | Ended                      | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| External            | Active            | Local                 | Active              | ❌                        | Active                     | [Empty]                        | Active                       | [Empty]                            | ✅                        |
| Local               | Active            | External              | Active              | ❌                        | Ended                      | "Prescribing error"            | Active                       | [Empty]                            | ❌                        |
| External            | Active            | Local                 | Active              | ✅                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |
| Local               | Active            | External              | Active              | ✅                        | Ended                      | "Prescribing error"            | Cancelled                    | "Prescribing error"                | ❌                        |

...to six small, easy-to-understand tests:

`MedicationCourseEnder.cs`
| **Should end** | **Expected status** | **Expected reason for ending** | **Expected to cancel issuances** | **Expected to log error** |
|:--------------:|:--------------------|:-------------------------------|:--------------------------------:|:-------------------------:|
| ✅             | Ended               | "Prescribing error"            | ✅                               | ❌                        |
| ❌             | Active              | [Empty]                        | ❌                               | ✅                        |

`MedicationIssuanceCanceller.cs`
| **Should cancel** | **Status** | **Expected status** | **Expected reason for cancelling** |
|:-----------------:|:-----------|:--------------------|:-----------------------------------|
| ✅                | Cancelled  | Cancelled           | "Prescribing error"                |
| ❌                | Active     | Cancelled           | [Empty]                            |

`ActiveCourseSelector.cs`
| **Status** | **Expected should end** |
|:-----------|:-----------------------:|
| Active     | ✅                      |
| Ended      | ❌                      |

`ActiveAndLocalCourseSelector.cs`
| **Added by** | **Status** | **Expected should end** |
|:-------------|:-----------|:-----------------------:|
| Local        | Active     | ✅                      |
| Local        | Ended      | ❌                      |
| External     | Active     | ❌                      |

`ActiveIssuanceSelector.cs`
| **Status** | **Expected should cancel** |
|:-----------|:--------------------------:|
| Active     | ✅                         |
| Ended      | ❌                         |

`ActiveAndLocalIssuanceSelector.cs`
| **Added by** | **Status** | **Expected should cancel** |
|:-------------|:-----------|:--------------------------:|
| Local        | Active     | ✅                         |
| Local        | Cancelled  | ❌                         |
| External     | Active     | ❌                         |

### Cognitive load

If we consider the above tests to be a representation of the cognitive load needed to understand each individual part of our system, we've clearly made huge improvements. If a bug is raised describing a problem in, for example, the wrong courses being ended, we immediately know that we need to look at the course selector classes. When we apply the principle to a real-world scenario, rather than this simplified example, the benefits are even more obvious.

### Reusability

Now that we've separated the ending of courses from the cancelling of issuances, we can use each one independently. Cancelling specific issuances can now be achieved without having to invoke any of the course ending code. We could, for example, cancel the most recent issuance of a course like so:

```csharp
var issuanceCanceller = new MedicationIssuanceCanceller(
    new ActiveIssuancesSelector());

issuanceCanceller.Cancel(new[] {course.Issuances.OrderBy(i => i.IssueDate) First()})
```

We could also end a course without cancelling any issuances, simply by creating another issuance canceller that just does nothing, and passing this into our `MedicationCourseEnder` class.

### Flexibility

The separating of the selection logic from the action logic has made our code a lot more flexible. We now have the ability to change each part without affecting the other, and can "plug in" different behaviours based on individual customer needs, without having to change any of the code being supported for existing customers.

## Conclusion

Hopefully this session has shown how important the single responsibility principle is to good software design. Following it well allows us to much more easily understand, change, and test our software.

In the next session, we'll be looking at the open/closed principle, and how applying it can allow us to introduce new features with minimal changes to our existing code.