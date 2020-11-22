module.exports = {
  extends: ["./commitlint.config.js"],
  defaultIgnores: false, // Don't allow things like `fixup!` when running against PRs
  ignores: [
    // Don't lint Dependabot commit messages because it doesn't reference an issue
    // and will fail linting
    (commit) => commit.startsWith("chore(deps"),
  ],
};
