Applying the SRP at the module level
====================================

Most discussions on the single responsibility focus only on applying the principle at a class or function level, but it's just as important that we apply the principle at higher levels too. This includes at the module level — in C#, this is at the project level — so let's start there!

## Non-conformity

Let's say we've created a project that will handle the UI elements of registering patients in a clinical system. We might have a project with classes and methods like the following:

<p align="center">
  <img src="./images/registration-module.png?raw=true"/>
</p>

At a high level, the module's responsibility is to allow the user to process patient registrations. The module contains a helper class with a method, `DisplayAge()`, that is used to display the patient's age in an appropriate format (e.g. years for adults; years & months for children; months & weeks for infants).

Does this module conform to the single responsibility principle? Does this module have only one potential reason to change?

**No.**

We'd need to change this module if there was:

1. A change to the way patients are registered.
2. A change to the way patient ages are displayed.

This module has **two** potential reasons to change, therefore violates the single responsibility principle.

## Issues that may arise

Since the module is taking an extra responsibility — particularly one that could be considered general-purpose enough to be used in many areas of the system — any other projects that wish to use the patient helper would need to do either of two things:

1. Add a reference to the entire Registration.Client project.

    This is less than ideal. Would it be appropriate to give an appointments module access to the code that displays a dialog to register a patient?

    I hope you'll agree that it wouldn't.

2. Copy and paste the `DisplayAge()` method into the project that needs it.

    This strategy is also sub-optimal. Any changes required to the way we display patient ages will now need to be made in two (or more) places. Can we guarantee that we'll remember to update every instance of this?

## Conforming to the principle

In order to conform to the principle, we'll need to move the `PatientHelper` class to a different project.

Since the `DisplayAge()` method is a general-purpose method that could be used in many areas, the best strategy is to move it to some non-domain-specific project so that it can be consumed by any project that needs it.

We could, instead, structure our system like the following:

<p align="center">
  <img src="./images/registration-module-conforming.png?raw=true"/>
</p>

Now, any project can display the patient ages using a consistent approach, and not be bloated by an entire project of unnecessary and inappropriate functionality.

Next we'll look at how we [apply the principle at the class/function level](./Readme-class-function-level.md)...