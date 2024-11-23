Applying the OCP at the function level
=======================================

In contrast to the SRP, let's start at the smallest entity involved in the principle - the function level.

Even if you've never come across the OCP before, if you've been coding for longer than a few months, chances are you've written code that conforms to it.

Let's say we're given a requirement that we must log a brief summary of a patient's medical record. We've been given the following acceptance criteria:

* As a user, I want the ability to print a report briefly outlining what types of medication a patient has been prescribed. The report should show
    * If the patient has been prescribed acute medication.
    * If the patient has been prescribed repeat medication.

## Implementation (not conforming to principle)

Let's look to implement this feature in the simplest way possible. We might throw together something like this:

```csharp
public void LogMedicationSummary(Boolean hasAcuteCourses, Boolean hasRepeatCourses)
{
    if (hasAcuteCourses)
        Console.WriteLine("The patient record contains acute courses.");

    if (hasRepeatCourses)
        Console.WriteLine("The patient record contains repeat courses.");
}
```

## Non-conformity

This satisfies our acceptance criteria, but it doesn't conform to the single responsibility principle. Let's take a look again at what it states:

> Software entities should be open for extension, but closed for modification.

or

> It should be possible to extend the behaviour of software without having to modify it.

When this code is released to a production environment, it is considered "closed for modification". 

If a future requirement is received asking for medication of other types to be summarised too, we'd have no choice but to modify this existing method.

This example is deliberately simple, but in real life we could be dealing wth something much more complex. Having to 