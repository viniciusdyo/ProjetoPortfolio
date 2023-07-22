const { glob, globSync, globStream, globStreamSync, Glob } = require('glob')

const path = require('path');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const RemoveEmptyScriptPlugin = require('webpack-remove-empty-scripts');
const TerserPlugin = require('terser-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const entry = {};
const jsEntry = {};
const cssEntry = {};

globSync('./js/*.js', { posix: true, dotRelative: true }).forEach(path => {
    const chunk = path.split('./js/')[1].split('.js')[0];
    jsEntry[chunk] = path;
});

globSync('./scss/*.scss', { posix: true, dotRelative: true }).forEach(path => {
    const chunk = path.split('./scss/')[1].split('.scss')[0];
    cssEntry[chunk] = path;
});

for (const jsItem in jsEntry) {
    for (const cssItem in cssEntry) {

        if (jsItem == cssItem) {

            const js = {};
            js[jsItem] = jsEntry[jsItem];

            const cs = {};
            cs[cssItem] = cssEntry[cssItem];

            entry[jsItem] = { import: [js[jsItem], cs[cssItem]], dependOn: 'shared' }

            delete jsEntry[jsItem];
            delete cssEntry[cssItem];
        }

        if (jsItem != null && entry[jsItem] == null) {
            entry[jsItem] = { import: jsEntry[jsItem], dependOn: 'shared' }
        }

        if (cssItem != null && entry[cssItem] == null) {
            entry[cssItem] = cssEntry[cssItem]
        }
    }
}
entry['shared'] = ['@splidejs/splide'];
module.exports = {
    mode: 'development',
    entry,
    output: {
        filename: '[name].min.js',
        path: path.resolve('./js/', 'build'),
    },
    plugins: [
        new RemoveEmptyScriptPlugin(),
        new MiniCssExtractPlugin({
            filename: '../../css/[name].min.css'
        }),
        new CssMinimizerPlugin(),
        new BundleAnalyzerPlugin({ openAnalyzer: false })
    ],
    module: {
        rules: [
            {
                test: /\.s[ac]ss$/i,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                    },
                    {
                        loader: 'css-loader',
                        options: {
                            url: true
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                plugins: () => [
                                    require('autoprefixer')
                                ]
                            }
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sassOptions: {
                                outputStyle: "compressed",
                            }
                        }
                    }
                ]
            },
            {
                test: /\.woff2?(\?v=\d+\.\d+\.\d+)?$/,
                type: 'asset/resource',
                generator: {
                    filename: './[name][ext][query]'
                }
            }
        ]
    },
    resolve: {
        extensions: ['*', '.js']
    },
    watch: true,
    optimization: {
        minimize: true,
        splitChunks: {
            chunks: 'all',
            minSize: 2000000,
        },
        usedExports: true,
        minimizer: [
            // `...`,
            new CssMinimizerPlugin(),
            new TerserPlugin({
                terserOptions: {
                    format: {
                        comments: false,
                    },
                    compress: {
                        drop_console: true,
                    },
                },
                extractComments: false,
            }),
        ]
    },
    devtool: 'eval', //comentar em producao
    stats: {
        assets: true,
        assetsSpace: 100,
        assetsSort: '!size'
    },
};