# --------------------------------------------------------------------------- #
#
# Copyright (c) 2010 CubeSoft, Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#  http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#
# --------------------------------------------------------------------------- #
require 'rake'
require 'rake/clean'
require 'fileutils'

# --------------------------------------------------------------------------- #
# configuration
# --------------------------------------------------------------------------- #
REPOSITORY  = 'Cube.FileSystem.SevenZip'
SUFFIX      = 'Ice'
NATIVE      = '../resources/native'
BRANCHES    = ['stable', 'net35']
PLATFORMS   = ['Any CPU', 'x86', 'x64']
CONFIGS     = ['Release', 'Debug']
COPIES      = ['Tests', 'Applications/Ice/Tests', 'Applications/Ice/Progress']
TESTCASES   = {
    "#{REPOSITORY}.Tests"     => 'Tests',
    "#{REPOSITORY}.Ice.Tests" => 'Applications/Ice/Tests'
}

# --------------------------------------------------------------------------- #
# commands
# --------------------------------------------------------------------------- #
BUILD   = 'msbuild /t:Clean,Build /m /verbosity:minimal /p:Configuration=Release;Platform="Any CPU";GeneratePackageOnBuild=false'
PACK    = 'nuget pack -Properties "Configuration=Release;Platform=AnyCPU"'
TEST    = '../packages/NUnit.ConsoleRunner/3.10.0/tools/nunit3-console.exe'

# --------------------------------------------------------------------------- #
# clean
# --------------------------------------------------------------------------- #
CLEAN.include("#{REPOSITORY}.*.nupkg")
CLEAN.include("../packages/cube.*")
CLEAN.include(%w{bin obj}.map{ |e| "**/#{e}" })

# --------------------------------------------------------------------------- #
# default
# --------------------------------------------------------------------------- #
desc "Clean objects and pack nupkg."
task :default => [:clean, :pack]

# --------------------------------------------------------------------------- #
# pack
# --------------------------------------------------------------------------- #
desc "Pack nupkg in the net35 branch."
task :pack do
    BRANCHES.each { |e| Rake::Task[:build].invoke(e) }
    sh("git checkout net35")
    sh("#{PACK} Libraries/#{REPOSITORY}.nuspec")
    sh("git checkout master")
end

# --------------------------------------------------------------------------- #
# test
# --------------------------------------------------------------------------- #
desc "Build and test projects in the current branch."
task :test => [:build] do
    fw  = `git symbolic-ref --short HEAD`.chomp
    fw  = 'net45' if (fw != 'net35')
    bin = ['bin', PLATFORMS[0], CONFIGS[0], fw].join('/')

    TESTCASES.each { |proj, root|
        dir = "#{root}/#{bin}"
        sh("#{TEST} \"#{dir}/#{proj}.dll\" --work=\"#{dir}\"")
    }
end

# --------------------------------------------------------------------------- #
# build
# --------------------------------------------------------------------------- #
desc "Build the solution in the specified branch."
task :build, [:branch] do |_, e|
    e.with_defaults(branch: '')
    sh("git checkout #{e.branch}") if (!e.branch.empty?)
    sh("nuget restore #{REPOSITORY}.#{SUFFIX}.sln")
    sh("#{BUILD} #{REPOSITORY}.#{SUFFIX}.sln")
end

# --------------------------------------------------------------------------- #
# copy
# --------------------------------------------------------------------------- #
desc "Copy resources to the bin directories."
task :copy do
    ['net45', 'net35'].product(PLATFORMS, CONFIGS) { |set|
        pf  = (set[1] == 'Any CPU') ? 'x64' : set[1]
        bin = ['bin', set[1], set[2], set[0]].join('/')
        COPIES.each { |root|
            dest = "#{root}/#{bin}"
            FileUtils.mkdir_p("#{dest}")
            FileUtils.cp_r(Dir.glob("#{NATIVE}/#{pf}/7z/7z.*"), "#{dest}")
        }
    }
end