# Umbraco 8.6.1 with reCAPTCHA v3 implementation utilizing ES6 features.

## Installation

Install packages with NPM

Open command line

change directory to WebApplication

Run ```npm install```

## Usage / GULP tasks

### Compile SASS
Run ```gulp sass``` concatenates scss in the _/src/sass_ folder, compiles it to css, minify and output to _/dist/css_ folder

Run ```gulp sass:watch``` watches for changes to scss files in _/src/sass_ folder

### Compile JS
Run ```gulp js``` transpile, minify, bundle and output to _/dist/js_ folder

Run ```gulp js:watch``` watches for changes to _/src/js_ folder

Run ```all:watch``` watches for changes to scss and js files in _/src/js_ and sass files in _/src/sass_, respectively

## Install Umbraco

Open the solution in Visual Studio or Rider. Build the solution and hit F5 or Ctrl + F5.

Install Umbraco. After Installation navigate to uSync back-office and run a full import.

## See reCAPTCHA v3 in action

Add the reCAPTCHA secret key to your app settings

```<add key="Recaptcha:Secret" value="" />```

Then navigate to the homepage and try the sign-up form.

Open developer tools (in the browser), then console and see your score.

## Future

I will build a NuGet package for it in the near future hopefully, so you can install it more easily in your Umbraco 
solution / project.