import * as React from 'react';
import { Route } from 'react-router';
import Header from './components/Header';
import Footer from './components/Footer';
import './index.scss';
import 'typeface-roboto';
import { applicationRoutes } from './applicationRoutes';

export default (): JSX.Element => (
  <div className="app-container">
    <Header />
    <main>
      {applicationRoutes.map((r) => (
        <Route
          key={r.path}
          path={r.path}
          component={r.component}
          exact={r.exact}
        />
      ))}
    </main>
    <Footer />
  </div>
);
