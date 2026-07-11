export default async function RootLayout({ children }) {
    return(
        <html data-scroll-behavior="smooth" suppressHydrationWarning={true}>
              <head>
                <link rel="preconnect" href="https://fonts.googleapis.com" />
                <link
                    rel="preconnect"
                    href="https://fonts.gstatic.com"
                    crossOrigin="true"
                />
                <link
                    href="https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap"
                    rel="stylesheet"
                ></link>
            </head>
            <body>
                {children}
            </body>
        </html>
    )

}