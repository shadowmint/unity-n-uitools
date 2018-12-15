# UiTools

This is a set of helpers and tools for working with unity UI components.

## Docs

Reference documentation:

- [RectTransform](https://github.com/shadowmint/unity-n-uitools/blob/master/docs/recttransform.md)

## Usage

See the `tests` folder for examples.

## Install

From your unity project folder:

    npm init
    npm install shadowmint/unity-n-uitools --save
    echo Assets/pkg-all >> .gitignore
    echo Assets/pkg-all.meta >> .gitignore

The package and all its dependencies will be installed in
your Assets/pkg-all folder.

## Development

Setup and run tests:

    npm install
    npm install ..
    cd test
    npm install

Remember that changes made to the test folder are not saved to the package
unless they are copied back into the source folder.

To reinstall the files from the src folder, run `npm install ..` again.
