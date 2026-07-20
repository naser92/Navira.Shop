"use client";

import { useContext, useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Layout from "@/layout";
import AccountProvider from "@/helper/accountContext/AccountProvider";
import AccountContext from "@/helper/accountContext/accountContext";
import SettingProvider from "@/helper/settingContext/SettingProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "../../public/assets/scss/app.scss";


function ProtectedContent({ children }) {
  const router = useRouter();
  const { isAuthenticated, isLoading } = useContext(AccountContext);
 
  useEffect(() => {
    if (!isLoading && !isAuthenticated) {
      router.replace("/auth/login");
    }
  }, [isAuthenticated, isLoading, router]);

  if (isLoading) {
    return <div className="p-4">Loading...</div>;
  }

  if (!isAuthenticated) {
    return null;
  }

  return <Layout>{children}</Layout>;
}

export default  function RootLayout({ children }) {
     const [queryClient] = useState(() => new QueryClient());
   
  return (
   
        <html dir="rtl" lang="fa" data-scroll-behavior="smooth" suppressHydrationWarning={true}>
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
                <QueryClientProvider client={queryClient}>
                    <SettingProvider>
                        <AccountProvider>
                            <ProtectedContent>{children}</ProtectedContent>
                        </AccountProvider>
                     </SettingProvider>
                 </QueryClientProvider>
            </body>
      </html>
     
  );
}


// export default async function RootLayout({ children }) {
//     return(
//         <html data-scroll-behavior="smooth" suppressHydrationWarning={true}>
//               <head>
//                 <link rel="preconnect" href="https://fonts.googleapis.com" />
//                 <link
//                     rel="preconnect"
//                     href="https://fonts.gstatic.com"
//                     crossOrigin="true"
//                 />
//                 <link
//                     href="https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap"
//                     rel="stylesheet"
//                 ></link>
//             </head>
//             <body>
//                 {children}
//             </body>
//         </html>
//     )

// }