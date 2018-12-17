# WorkPeriodAlgorithm

This repository has 2 projects in her repository. The Work Period Time Calculator Algorithm and its reverse.

The Working Period allows you to set time of start of work in a day and the expected ending. It also allows you to set a time for Users to log requests or complaints and the expected endtime where any logged request or complaint after this period is automatically carried to the start of the next day's business hour.

The Algorithm uses these set dates together with a set holiday date or work-free day and the turn around time to resolve or attend to complaints such that their expected end of resolution time is know.

If a ticket is logged for eample on a Saturday and if there is not work then, it takes the ticket to a starting date of Monday when work resumes and calculate the ending datetime using the variables such as turn around time (TAT), holiday/work free date, and the start date.

This algorithm can also be used to attend to other types of solutions as the user deems fit.
