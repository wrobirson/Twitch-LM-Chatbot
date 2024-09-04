'use client'

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { MessageSquare, Twitch, Zap, Shield, Code } from "lucide-react"

export function LandingPage() {
  return (
      <div className={'container mx-auto'}>
        <div className="flex flex-col min-h-screen">
          <header className="px-4 lg:px-6 h-14 flex items-center">
            <a className="flex items-center justify-center" href="/">
              <img src='/chatbot.png' className="h-6 w-6" alt={'Logo'}/>
              <span className="ml-2 text-2xl font-bold text-gray-900">Twitch LM Chatbot</span>
            </a>
            <nav className="ml-auto flex gap-4 sm:gap-6">
              <a className="text-sm font-medium hover:underline underline-offset-4" href="#features">
                Features
              </a>
              <a className="text-sm font-medium hover:underline underline-offset-4" href="#how-it-works">
                How It Works
              </a>
              <a className="text-sm font-medium hover:underline underline-offset-4" href="#get-started">
                Get Started
              </a>
            </nav>
          </header>
          <main className="flex-1">
            <section className="w-full py-12 md:py-24 lg:py-32 xl:py-48 bg-purple-100">
              <div className="container px-4 md:px-6">
                <div className="flex flex-col items-center space-y-4 text-center">
                  <div className="space-y-2">
                    <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl lg:text-6xl/none">
                      Enhance Your Twitch Stream with AI-Powered Chat
                    </h1>
                    <p className="mx-auto max-w-[700px] text-gray-600 md:text-xl">
                      Twitch LM Chatbot uses advanced language models to interact with your viewers and create engaging
                      experiences.
                    </p>
                  </div>
                  <div className="space-x-4">
                    <Button asChild>
                      <a href="https://wrobirson.itch.io/twitch-lm-chatbot">Get Started</a>
                    </Button>
                    <Button variant="outline" asChild>
                      <a href="#features">Learn More</a>
                    </Button>
                  </div>
                </div>
              </div>
            </section>
            <section id="features" className="w-full py-12 md:py-24 lg:py-32">
              <div className="container px-4 md:px-6">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl text-center mb-12">Features</h2>
                <div className="grid gap-10 sm:grid-cols-2 lg:grid-cols-3">
                  <div className="flex flex-col items-center text-center">
                    <MessageSquare className="h-12 w-12 text-purple-500 mb-4"/>
                    <h3 className="text-xl font-bold mb-2">AI-Powered Interactions</h3>
                    <p className="text-gray-600">Engage your viewers with intelligent, context-aware responses.</p>
                  </div>
                  <div className="flex flex-col items-center text-center">
                    <Shield className="h-12 w-12 text-purple-500 mb-4"/>
                    <h3 className="text-xl font-bold mb-2">Smart Moderation</h3>
                    <p className="text-gray-600">Automatically moderate chat to keep your stream friendly and safe.</p>
                  </div>
                  <div className="flex flex-col items-center text-center">
                    <Zap className="h-12 w-12 text-purple-500 mb-4"/>
                    <h3 className="text-xl font-bold mb-2">Custom Commands</h3>
                    <p className="text-gray-600">Create personalized commands to enhance viewer interaction.</p>
                  </div>
                </div>
              </div>
            </section>
            <section id="how-it-works" className="w-full py-12 md:py-24 lg:py-32 bg-gray-100">
              <div className="container px-4 md:px-6">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl text-center mb-12">How It
                  Works</h2>
                <div className="grid gap-6 lg:grid-cols-3">
                  <div className="flex flex-col items-center text-center p-4 bg-white rounded-lg shadow-md">
                    <div className="rounded-full bg-purple-100 p-3 mb-4">
                      <Twitch className="h-6 w-6 text-purple-500"/>
                    </div>
                    <h3 className="text-xl font-bold mb-2">1. Connect to Twitch</h3>
                    <p className="text-gray-600">Easily integrate the chatbot with your Twitch channel.</p>
                  </div>
                  <div className="flex flex-col items-center text-center p-4 bg-white rounded-lg shadow-md">
                    <div className="rounded-full bg-purple-100 p-3 mb-4">
                      <Code className="h-6 w-6 text-purple-500"/>
                    </div>
                    <h3 className="text-xl font-bold mb-2">2. Configure Settings</h3>
                    <p className="text-gray-600">Customize the chatbot's behavior to fit your stream's needs.</p>
                  </div>
                  <div className="flex flex-col items-center text-center p-4 bg-white rounded-lg shadow-md">
                    <div className="rounded-full bg-purple-100 p-3 mb-4">
                      <MessageSquare className="h-6 w-6 text-purple-500"/>
                    </div>
                    <h3 className="text-xl font-bold mb-2">3. Engage Your Audience</h3>
                    <p className="text-gray-600">Let the AI-powered chatbot interact with your viewers in real-time.</p>
                  </div>
                </div>
              </div>
            </section>
            <section id="get-started" className="w-full py-12 md:py-24 lg:py-32">
              <div className="container px-4 md:px-6">
                <div className="flex flex-col items-center space-y-4 text-center">
                  <div className="space-y-2">
                    <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                      Ready to Elevate Your Twitch Stream?
                    </h2>
                    <p className="mx-auto max-w-[600px] text-gray-600 md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                      Get started with Twitch LM Chatbot today and transform your streaming experience.
                    </p>
                  </div>
                  <div className="w-full max-w-sm space-y-2">
                    <a href="https://wrobirson.itch.io/twitch-lm-chatbot">
                      <Button size={'lg'} variant={'default'}>Download</Button>
                    </a>
                  </div>
                </div>
              </div>
            </section>
          </main>
          <footer className="flex flex-col gap-2 sm:flex-row py-6 w-full shrink-0 items-center px-4 md:px-6 border-t">
            <p className="text-xs text-gray-500">© 2023 Twitch LM Chatbot by <a
                href="https://github.com/wrobirson">wrobirson</a> All rights reserved.</p>
            <nav className="sm:ml-auto flex gap-4 sm:gap-6">

            </nav>
          </footer>
        </div>
      </div>
  )
}