module.exports = {
  extends: ["./commitlint.config.js"],
  defaultIgnores: false, // Don't allow things like `fixup!` when running against PRs
};
