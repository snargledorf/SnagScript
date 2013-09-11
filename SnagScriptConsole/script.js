function Driver(name, age) { // Named function declaration
    this.Name = name;
    this.Age = age;
    this.isOldEnoughToDrive = function () { // Anonymous function expression

        printLine("Determining if driver is of legal age...");
        for (var i = 1; i <= 10; i++) {
            printLine(i);
            pause(500);
        }

        clearScreen();

        return this.Age >= 16;
    };
}

driverTest = function () {

    clearScreen();

    printLine("The Driver function has " + Driver.length + " arguments"); // Default properties
    print("Press any key to continue...");
    readKey();

    var exit = false;
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
        catch (ex) {
            clearScreen();
            printLine("ERROR: " + ex.message);
            print("Press any key to try again...");
            readKey();
        }
    }
}

customFunctionTest = function () {

    clearScreen();

    var two = 2;

    printLine("Two: " + two);

    var twoSquared = square(2);

    printLine("Two squared: " + twoSquared);
}

runOption = function (option) {
    if (option == 1) {
        driverTest();
    } else if (option == 2) {
        customFunctionTest();
    } else if (option == 3) {
        exit();
    }
}

optionIsValid = function (option) {
    return option == 1 || option == 2 || option == 3;
}

while (true)
{
    clearScreen();

    printLine("Main Menu:");
    printLine();

    printLine("Please enter one of the options below");
    printLine("1) Driver Test");
    printLine("2) Custom Function Test");
    printLine("3) Exit");

    var option = parseInt(readKey());
    
    if (optionIsValid(option))
    {
        runOption(option);

        print("Press any key to return to the Main Menu...");

        readKey();
    }
}