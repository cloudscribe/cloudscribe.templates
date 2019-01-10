const path = require('path');
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const config = {
    entry: {
        // each entry defines a bundle that will be produced
        'app-react': './app-react/boot-client.tsx',
        'app-react-server': './app-react/boot-server.tsx',
        'vanilla': './app-vanilla/Main.ts'
        
    },
    mode: 'development',
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist/'),
        publicPath: '/dist/'
    },
    resolve: {
        modules: [path.join(__dirname,"./node_modules/")],
        extensions: [".tsx", ".ts", ".js", ".scss"]
    },
    devtool: 'source-map',
    externals: {
        // require("jquery") is external and available
        //  on the global var jQuery
        "jquery": "jQuery"
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader"
                ],
                exclude: /node_modules/
            },
            //{ 
            //    test: /\.(sass|scss)$/,
            //    use: ExtractTextPlugin.extract({
            //        fallback: 'style-loader',
            //        use: ['css-loader', 'sass-loader']
            //    }), 
           // },
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/,
            },
        ]
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NamedModulesPlugin(),
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional
            //filename: devMode ? '[name].css' : '[name].[hash].css',
            filename: '[name].bundle.css',
            chunkFilename: "[id].css"
            //chunkFilename: devMode ? '[id].css' : '[id].[hash].css',
        })
       
       
    ]
};

module.exports = config;