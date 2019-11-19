import React from 'react';
import './App.css';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

const App: React.FC = () => {
  return (
    <Layout>
      <Route exact path='/' component={Home} />
    </Layout>
  );
}

export default App;
