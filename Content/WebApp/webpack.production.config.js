const webpack = require("webpack");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const merge = require('webpack-merge');
const CompressionPlugin = require('compression-webpack-plugin');
const devConfig = require('./webpack.config.js');

var devFiltered = devConfig;
devFiltered.plugins = []; //remove dev plugins, we don't want to merge with them we want to replace them

const overrides = {
    mode: 'production',
    output: {
        filename: '[name].bundle.min.js',
    },
    optimization: {
        minimizer: [
            new UglifyJsPlugin({
                cache: true,
                parallel: true,
                uglifyOptions: {
                    compress: false,
                    ecma: 5,
                    mangle: true
                },
                sourceMap: false
            })
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional
            //filename: devMode ? '[name].css' : '[name].[hash].css',
            filename: '[name].bundle.min.css',
            chunkFilename: "[id].min.css"
            //chunkFilename: devMode ? '[id].css' : '[id].[hash].css',
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        }),
        new CompressionPlugin({   
            asset: "[path].gz[query]",
                algorithm: "gzip",
                test: /\.js$|\.css$|\.html$/,
                threshold: 10240,
                minRatio: 0.8
            })
    ]
      
};

const merged = merge(devFiltered, overrides);

module.exports = merged;
