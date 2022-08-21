SOLID Principles
================

## Introduction

The SOLID principles are a set of design principles devised by Robert C. Martin ("Uncle Bob") to help guide developers towards the best way to design clean, maintainable, extendable and testable software.

As an engineering discipline, software engineering is unique in the level of change to which it is subjected. If we design poor-quality software, it's this element of change that will cause problems for our end users, and us!

When applied correctly, the SOLID principles can help us to not only improve the quality of our software, but also to build that software more efficiently.

## What are the principles?

The five principles spell out the word "SOLID" and are as follows:

* [Single responsibility principle](./SolidPrinciples/SRP/Readme.md)
* Open-closed principle
* Liskov substitution principle
* Interface segregation principle
* Dependency inversion principle

Over the coming sessions, we'll go over each one in turn, and describe in detail the problems that can occur when an opportunity to apply each principle is missed.

## When is it appropriate to apply the principles?

The principles should be seen as recommended guidelines, not something that should be followed religiously.

For example, spending an entire sprint making something extendable that likely will never be extended is not an efficient use of our engineering time.

It's important that we work pragmatically and find the right balance. A good understanding of the SOLID principles also brings with it an understanding of when to apply each one.

## The sessions

Each session will focus on one of the five principles.

We'll look at simplified examples of C# code that would benefit from the application of the principle in question. We'll then look at the problems that not applying said principle could cause, and finally see a version of the same code with the principle applied.

------------------------

Let's get started on the first principle, the [single responsibility principle](./SolidPrinciples/SRP/Readme.md)...