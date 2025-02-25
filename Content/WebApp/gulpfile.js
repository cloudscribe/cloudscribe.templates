"use strict";

var gulp = require("gulp"),
    concat = require("gulp-concat"),
    postcss = require('gulp-postcss'),
    cssnano = require('cssnano'), 
    gp_rename = require('gulp-rename'),
    uglify = require("gulp-uglify"),
    sass = require('gulp-sass')(require('sass')),
    merge = require('merge-stream')
;

var config = {
    srcSassDir: './app-scss',
    cssOutDir: './sitefiles/s1/themes/custom1/wwwroot/css',
    srcFileWatchPattern: './app-scss/*.scss'
};

gulp.task('buildCustom1ThemeCss', function () {
    return gulp.src(config.srcSassDir + '/style.scss', { sourcemaps: true })
    .pipe(sass({
        //outputStyle: 'compressed',
        includePaths: [
            config.srcSassDir
        ],
    }).on('error', sass.logError)
        )
        .pipe(gulp.dest(config.cssOutDir))
        .pipe(gp_rename('style.min.css'))
        // .pipe(cssmin())   -- deprecated - replaced below
        .pipe(postcss([cssnano({
            preset: ['default', { discardComments: { removeAll: true } }]
        })]))
        .pipe(gulp.dest(config.cssOutDir));
});

// if you run the default task it will watch for changes in files and then run the
// array of tasks if any files changed. So for scss changes you can just refresh the page to see changes
gulp.task('default', function () {
    gulp.watch(config.srcFileWatchPattern, gulp.series('buildCustom1ThemeCss'));
});
