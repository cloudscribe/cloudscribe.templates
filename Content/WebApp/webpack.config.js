const path = require('path');
const webpack = require("webpack");
// https://www.npmjs.com/package/webpack-livereload-plugin
const LiveReloadPlugin = require('webpack-livereload-plugin');

module.exports = {
    mode: 'production',
    performance: {
        maxEntrypointSize: 512000,
        maxAssetSize: 512000
    },
    entry: {
        reactdemo1: ['./Scripts/React/Demo1/demo1.tsx'],
        reactdemo2: ['./Scripts/React/Demo2/demo2.tsx'],
    },
    output: {
        filename: '[name].bundle.js',
        libraryTarget: 'var',
        library: '[name]',
        path: path.resolve(__dirname, 'wwwroot/js'),
        // path: path.resolve(__dirname, 'sitefiles/s1/themes/custom1/wwwroot/js'),
        publicPath: '/dist/'
    },
    plugins: [
        new LiveReloadPlugin({

        })
    ],
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.mjs$/,
                include: /node_modules/,
                type: 'javascript/auto'
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
                exclude: /node_modules/
            }
            // --- if you want to bundle sass/scss...
            // {
            //     test: /\.css$/,
            //     use: ExtractTextPlugin.extract({
            //         fallback: "style-loader",
            //         use: "css-loader"
            //     }),
            //     //exclude: /node_modules/
            // },
            // {
            //    test: /\.(sass|scss)$/,
            //    use: ExtractTextPlugin.extract({
            //        fallback: 'style-loader',
            //        use: ['css-loader', 'sass-loader']
            //    }), 
            // }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js", ".mjs", ".cjs"]
    },
    
    devtool: 'source-map'
};