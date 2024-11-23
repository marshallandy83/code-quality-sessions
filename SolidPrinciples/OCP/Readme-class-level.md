Applying the OCP at the class level
=======================================

Let's imagine we've been given the following requirements:

Display a list of the patient's past and current medication courses. Allow the user to perform the following actions when selecting courses:

1. If a single active course is selected, the following actions should be available:

* End
* Edit
* Issue
* View course history

2. If multiple active courses are selected, the following actions should be available:

* End

3. If a single ended course is selected, the following actions should be available:

* Restart
* View course history

4. If multiple ended courses are selected, the following actions should be available:

* Restart

5. If the selected courses contain a mixture of active and ended courses, no actions should be available.