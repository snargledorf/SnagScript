function Driver(name, age) {
    this.Name = name;
    this.Age = age;
    this.isOldEnoughToDrive = function () {

        printLine("Determining if driver is of legal age...");
        for (var i = 1; i <= 10; i++) {
            printLine(i);
            pause(500);
        }

        clearScreen();

        return this.Age >= 16;
    };
}

exit = false;
while (exit == false) {
    try {
        clearScreen();

        print("Input your name: ");
        var name = readLine();
        clearScreen();

        print("How old are you " + name + "?: ");
        var age = parseInt(readLine());
        clearScreen();

        printLine("You are " + age + " years old");

        var driver = new Driver(name, age);

        if (driver.isOldEnoughToDrive()) {
            printLine("You are old enough to drive");
        } else {
            printLine("Pull Over!!!");
        }

        exit = true;
    } 
    catch (ex)
    {
        clearScreen();
        printLine("ERROR: " + ex.message);
        print("Press any key to try again...");
        readKey();
    }
}

print("Press any key to exit...");
readKey();