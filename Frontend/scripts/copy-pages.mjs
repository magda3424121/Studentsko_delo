import { copyFileSync } from "node:fs";
import { resolve } from "node:path";

const outputDirectory = resolve("../Backend/wwwroot");
const builtIndex = resolve(outputDirectory, "index.html");
const pageAliases = [
  "prijava.html",
  "registracija-student.html",
  "prijava-podjetje.html",
  "registracija-podjetje.html",
  "podjetja-index.html",
  "admin-prijava.html",
  "admin.html",
  "za-studente.html",
  "za-podjetje.html",
];

for (const pageAlias of pageAliases) {
  copyFileSync(builtIndex, resolve(outputDirectory, pageAlias));
}
